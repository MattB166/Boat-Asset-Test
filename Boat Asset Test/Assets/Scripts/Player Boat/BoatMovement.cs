using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    [HideInInspector] public Vector2 input;
    public InputActionAsset inputActions;
    private InputAction accelerateAction;
    private InputAction steerAction;
    private Rigidbody rb;
    public bool accelerating = false;
    public bool steering = false;
    public Transform motor;
    public float maxSpeed;
    public float speedAccelerationRate;
    private float throttle;
    public float maxEngineForce;
    private float mphSpeed;
    private float kmhSpeed;
    private float msSpeed;

    public float minSteerSpeed;
    public float maxSteerAngle;
    public float steerSensitivity;
    public float turnStrength;
    private float currentRudderAngle;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.LogError("Rigidbody component not found on boat.");
        }
        accelerateAction = inputActions.FindAction("Accelerate");
        steerAction = inputActions.FindAction("Steer");
        
    }
    private void OnEnable()
    {
        accelerateAction.Enable();
        steerAction.Enable();
    }

    private void OnDisable()
    {
        accelerateAction.Disable();
        steerAction.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        throttle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current speed: " + rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        //throttle = Mathf.Clamp01(throttle);

        if(accelerating)
        {
           throttle += speedAccelerationRate * Time.fixedDeltaTime;
        }
        else
        {
            throttle -= speedAccelerationRate * Time.fixedDeltaTime;

        }
        throttle = Mathf.Clamp01(throttle);

        float speedFactor = 1f - (rb.velocity.magnitude / maxSpeed);
        speedFactor = Mathf.Clamp01(speedFactor);
        float lowSpeedBoost = Mathf.Clamp01(1f - (rb.velocity.magnitude / 10f));

        float engineForce = throttle * maxEngineForce * speedFactor * (1f + lowSpeedBoost);

        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
        localVel.x *= 0.5f; // reduce sideways velocity for better handling
        rb.velocity = transform.TransformDirection(localVel);

        //Debug.Log("Throttle: " + throttle + ", Engine Force: " + engineForce);

        Debug.Log("current speed: " + rb.velocity.magnitude);

        rb.AddForce(transform.forward * engineForce, ForceMode.Acceleration);

        

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            //Debug.Log("Max speed reached, clamping velocity.");
        }

        CalculateSpeed(rb);

        Steer();    
    }

    public void OnSteerInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            input = context.ReadValue<Vector2>();
            steering = true;
        }
        if(context.canceled)
        {
            steering = false;
        }

        //Debug.Log("Steer input: " + input);

        //take into account velocity, mass, drag, and steering sensitivity to apply a torque to the boat's rigidbody for turning. also consider the current speed of the boat to adjust the steering response.
        //higher the speed,less responsive the steering should be to prevent oversteering at high speeds. also consider the direction of the input to determine whether to apply a positive or negative torque for left or right turns.


    }

    public void OnAccelerateInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //accelerate the boat forward by applying a force to the rigidbody in the forward direction, multiplied by the input value, and a speed multiplier. take into account mass, throttle, engine size.
             //Debug.Log("Accelerate input: " + context.ReadValue<float>());
             accelerating = true;
        }
        if(context.canceled)
        {
            accelerating = false;
        }
    }


    private void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(motor.position, motor.position + motor.forward * 5f);

            Gizmos.color = Color.blue;  
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5f);

        if(Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity.normalized * 5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(motor.position, motor.position + rb.velocity.normalized * 5f);
        }
    }


    public void CalculateSpeed(Rigidbody rb)
    {
        mphSpeed = rb.velocity.magnitude * 2.23694f; // convert m/s to mph
        kmhSpeed = rb.velocity.magnitude * 3.6f; // convert m/s to km/h
        msSpeed = rb.velocity.magnitude; // speed in m/s
    }

    public float GetSpeedMPH()
    {
        mphSpeed = rb.velocity.magnitude * 2.23694f; // convert m/s to mph
        return mphSpeed;
    }



    public void Steer()
    {
        if(rb.velocity.magnitude < minSteerSpeed)
        {
            return; // don't allow steering at very low speeds to prevent unrealistic turning in place
        }
        if (steering)
        {
            float steerInput = input.x;
            float speed = rb.velocity.magnitude;
            float speed01 = Mathf.Clamp01(speed / maxSpeed);
            //allow slower turning at static and higher speeds, best turning at medium speeds.
            currentRudderAngle = Mathf.Lerp(currentRudderAngle, steerInput * maxSteerAngle, Time.fixedDeltaTime * steerSensitivity);

            //apply torque to the boat based on the current rudder angle and the speed of the boat, with a turn strength multiplier to control how responsive the steering is. also consider the direction of the input to determine whether to apply a positive or negative torque for left or right turns.
            float turnTorque = currentRudderAngle * turnStrength * (1f - speed01);
            rb.AddTorque(transform.up * turnTorque, ForceMode.Acceleration);




        }
    }
    
}

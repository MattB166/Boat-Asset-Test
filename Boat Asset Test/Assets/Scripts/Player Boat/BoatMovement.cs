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
    public float maxSpeedMS = 20f;
    public float accelerationRate;
    private float throttle;
    public float maxEngineForce;
    private float mphSpeed;
    private float kmhSpeed;
    private float msSpeed;


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
        throttle = Mathf.Clamp01(throttle);

        if(accelerating)
        {
           throttle += accelerationRate * Time.fixedDeltaTime;
        }
        else
        {
            
            throttle -= accelerationRate * Time.fixedDeltaTime;
        }


        float speedFactor = 1f - (rb.velocity.magnitude / maxSpeedMS);
        speedFactor = Mathf.Clamp01(speedFactor);
        float lowSpeedBoost = Mathf.Clamp01(1f - (rb.velocity.magnitude / 10f));

        float engineForce = throttle * maxEngineForce * speedFactor * (1f + lowSpeedBoost);

        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
        localVel.x *= 0.5f; // reduce sideways velocity for better handling
        rb.velocity = transform.TransformDirection(localVel);

        //Debug.Log("Throttle: " + throttle + ", Engine Force: " + engineForce);

        Debug.Log($"Speed m/s: {rb.velocity.magnitude}, mph: {GetSpeedMPH()}, throttle : {throttle}");

        rb.AddForce(transform.forward * engineForce, ForceMode.Acceleration);

        //rb.AddForce(-rb.velocity * 0.03f, ForceMode.Acceleration); // simple drag to prevent infinite acceleration

        if(rb.velocity.magnitude > maxSpeedMS)
        {
            rb.velocity = rb.velocity.normalized * maxSpeedMS;
            Debug.Log("Max speed reached, clamping velocity.");
        }

        CalculateSpeed(rb);
    }

    public void OnSteerInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        //Debug.Log("Steer input: " + input);
        steering = true;
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
}

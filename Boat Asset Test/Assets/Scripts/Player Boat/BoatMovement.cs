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

    public Transform startPos;

    private Collider boatCollider;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.LogError("Rigidbody component not found on boat.");
        }
        accelerateAction = inputActions.FindAction("Accelerate");
        steerAction = inputActions.FindAction("Steer");

        boatCollider = GetComponent<Collider>();

        if(startPos != null)
            InitialisePlayer();



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
        
        
    }

    public void InitialisePlayer()
    {
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
            
    }

    private void FixedUpdate()
    {
        

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
        //float lowSpeedBoost = Mathf.Clamp01(1f - (rb.velocity.magnitude / 10f));

        float engineForce = throttle * maxEngineForce * speedFactor;

        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
        localVel.x *= 0.5f; 
        rb.velocity = transform.TransformDirection(localVel);

        

        rb.AddForce(transform.forward * engineForce, ForceMode.Acceleration);

        

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            
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

        


    }

    public void OnAccelerateInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            
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
        mphSpeed = rb.velocity.magnitude * 2.23694f; 
        kmhSpeed = rb.velocity.magnitude * 3.6f; 
        msSpeed = rb.velocity.magnitude; 
    }

    public float GetSpeedMPH()
    {
        mphSpeed = rb.velocity.magnitude * 2.23694f; 
        return mphSpeed;
    }



    public void Steer()
    {
            float speed = rb.velocity.magnitude;
        Debug.Log("Current speed: " + speed);
        if (speed < minSteerSpeed)
        {
            rb.angularVelocity = Vector3.zero;
            return; 
        }
        if (steering)
        {
            float steerInput = input.x;
            float speed01 = Mathf.Clamp01(speed / maxSpeed);
            
            currentRudderAngle = Mathf.Lerp(currentRudderAngle, steerInput * maxSteerAngle, Time.fixedDeltaTime * steerSensitivity);


            float steeringMultiplier = Mathf.Lerp(1f, 0.5f, speed01);

            float turnTorque = currentRudderAngle * turnStrength * steeringMultiplier;
            rb.AddRelativeTorque(Vector3.up * turnTorque, ForceMode.Acceleration);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Boat collided with obstacle.");
            InitialisePlayer();
        }
    }

}

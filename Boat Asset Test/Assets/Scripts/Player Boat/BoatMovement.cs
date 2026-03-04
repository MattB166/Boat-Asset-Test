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
    public float enginePower = 10000f;
    public float maxSpeed = 20f;
    public bool accelerating = false;
    public bool steering = false;
    public Transform motor;
    public float steerPower = 500f;
    public float Power;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current speed: " + rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {

        

    }

    public void OnSteerInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        Debug.Log("Steer input: " + input);
        steering = true;
        //take into account velocity, mass, drag, and steering sensitivity to apply a torque to the boat's rigidbody for turning. also consider the current speed of the boat to adjust the steering response.
        //higher the speed,less responsive the steering should be to prevent oversteering at high speeds. also consider the direction of the input to determine whether to apply a positive or negative torque for left or right turns.


    }

    public void OnAccelerateInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //accelerate the boat forward by applying a force to the rigidbody in the forward direction, multiplied by the input value, and a speed multiplier. take into account mass, throttle, engine size.
             Debug.Log("Accelerate input: " + context.ReadValue<float>());
             accelerating = true;
        }
        if(context.canceled)
        {
            accelerating = false;
        }
    }


   
}

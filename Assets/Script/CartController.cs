using System;
using UnityEngine;

public class CartController : MonoBehaviour
{
    [Header("Car Properties")]
    [SerializeField] float motorTorque = 1000f;
    [SerializeField] float brakeTorque = 2000f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float steeringRange = 45f;
    [SerializeField] float steeringRangeAtMaxSpeed = 10f;
    [SerializeField] float centreOfGravityOffset = -1f;
    [SerializeField] private Transform steer;
    [SerializeField] private float maxSteerRotation = 45f;
    [SerializeField] float steerRotationSpeed = 90f; //g/s
    [SerializeField] WheelControl[] wheels;

    // The new input system (checkbox on: Generate C# class fo InputSystem_Actions file in project)
    private InputSystem_Actions _carControls;
    private Rigidbody _rigidBody;

    //Steer informations
    private Quaternion _steerStartRotation;
    private float _steerCurrentYRotation;

    //Input system
    private Vector2 _inputVector;
    private float _vInput; // Forward/backward input
    private float _hInput; // Steering input

    //Notify about car speed
    public static Action<float> CarSpeed;

    void Awake()
    {
        _steerStartRotation = steer.localRotation; //LOCAL ROTATION OF STEER
        _carControls = new InputSystem_Actions(); // Initialize Input Actions
    }
    void OnEnable()
    {
        _carControls.Enable();
    }

    void OnDisable()
    {
        _carControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass to improve stability and prevent rolling
        Vector3 centerOfMass = _rigidBody.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        _rigidBody.centerOfMass = centerOfMass;

        // Get all wheel components attached to the car
        wheels = GetComponentsInChildren<WheelControl>();
    }

    private void Update()
    {
        // Read the Vector2 input from the new Input System
        Vector2 inputVector = _carControls.Car.Mouvement.ReadValue<Vector2>();

        //Debug.Log($"vInput: {vInput}, hInput: {hInput}");

        // Get player input for acceleration and steering
        _vInput = inputVector.y; // Forward/backward input
        _hInput = inputVector.x; // Steering input

        //Rotate Steer
        _steerCurrentYRotation = Mathf.MoveTowards(_steerCurrentYRotation, _hInput * maxSteerRotation, steerRotationSpeed * Time.deltaTime);
        //Debug.Log($"{_hInput*maxSteerRotation} -> {_steerCurrentYRotation}");

        Quaternion targetRotation = Quaternion.Euler(0f, _steerCurrentYRotation, 0f);
        steer.localRotation = _steerStartRotation * targetRotation; //quaternion multiplication = combine rotations (first then second term) 
    }

    // FixedUpdate is called at a fixed time interval
    void FixedUpdate()
    {
        // Calculate current speed along the car's forward axis
        float forwardSpeed = Vector3.Dot(transform.forward, _rigidBody.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed)); // Normalized speed factor

        //Notify the speedometer
        CarSpeed?.Invoke(speedFactor);

        // Reduce motor torque and steering at high speeds for better handling
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Determine if the player is accelerating or trying to reverse
        bool isAccelerating = Mathf.Approximately(Mathf.Sign(_vInput), Mathf.Sign(forwardSpeed));

        foreach (var wheel in wheels)
        {
            // Apply steering to wheels that support steering
            if (wheel.Steerable)
            {
                wheel.WheelCollider.steerAngle = _hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to motorized wheels
                if (wheel.Motorized)
                {
                    wheel.WheelCollider.motorTorque = _vInput * currentMotorTorque;
                }
                // Release brakes when accelerating
                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                // Apply brakes when reversing direction
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = Mathf.Abs(_vInput) * brakeTorque;
            }
        }
    }

    /*public void Speedometer(){
        float circumference = 2.0f * 3.14f * wheels[0].WheelCollider.radius; // Finding circumFerence 2 Pi R
        float speedOnKmh = (circumference * wheels[0].WheelCollider.rpm)*60; 

        Debug.Log($"{speedOnKmh} - {speedOnKmh/1000f} - {wheels[0].WheelCollider.rpm}");

        CarSpeed?.Invoke(speedFactor);// finding kmh
    }*/
}

using UnityEngine;
using UnityEngine.EventSystems;

public class AdvancedTruckController : MonoBehaviour
{
    public float maxMotorTorque = 3000f;  // Maximum torque the motor can apply
    public float maxSteeringAngle = 100f;  // Maximum steering angle for the front wheels
    public float brakeForce = 5000f;      // Brake force applied when braking
    public Transform centerOfMass;        // Center of mass to stabilize the truck

    // Wheel Colliders
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    // Input
    private float motorInput;
    private float steeringInput;
    private float brakeInput;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set the truck's center of mass lower to prevent it from flipping easily
        if (centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }
    }

    void Update()
    {
        // Get player inputs for acceleration, steering, and braking
        motorInput = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow for acceleration and reverse
        steeringInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow for steering
        brakeInput = Input.GetKey(KeyCode.Space) ? brakeForce : 0f;  // Space key for braking
    }

    void FixedUpdate()
    {
        // Control truck movement and apply forces to the wheels
        HandleMotor();
        HandleSteering();
        ApplyBrakes();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        // Apply torque to the rear wheels based on input
        float motorTorque = motorInput * maxMotorTorque;

        wheelRL.motorTorque = motorTorque;
        wheelRR.motorTorque = motorTorque;
    }

    private void HandleSteering()
    {
        // Apply steering to the front wheels based on input
        float steeringAngle = steeringInput * maxSteeringAngle;

        wheelFL.steerAngle = steeringAngle;
        wheelFR.steerAngle = steeringAngle;
    }

    private void ApplyBrakes()
    {
        // Apply braking force to all wheels when braking
        wheelRL.brakeTorque = brakeInput;
        wheelRR.brakeTorque = brakeInput;

        // Optionally apply a smaller brake force to the front wheels for more realistic braking
        wheelFL.brakeTorque = brakeInput * 0.5f;
        wheelFR.brakeTorque = brakeInput * 0.5f;
    }

    private void UpdateWheels()
    {
        // Update the visual position of the wheels based on the colliders
        UpdateWheelPosition(wheelFL);
        UpdateWheelPosition(wheelFR);
        UpdateWheelPosition(wheelRL);
        UpdateWheelPosition(wheelRR);
    }

    private void UpdateWheelPosition(WheelCollider collider)
    {
        // This moves the 3D model of the wheels based on the collider's position
        Vector3 pos;
        Quaternion rot;

        collider.GetWorldPose(out pos, out rot);

        // Assuming you have separate wheel meshes, adjust their position/rotation here
        // For example: wheelFLMesh.transform.position = pos; wheelFLMesh.transform.rotation = rot;
    }
}

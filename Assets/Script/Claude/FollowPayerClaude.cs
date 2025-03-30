using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float height = 5f;
    public float angle = 45f;
    public float orbitSpeed = 5f;
    public float minVerticalAngle = 20f;
    public float maxVerticalAngle = 80f;
    public float smoothSpeed = 0.125f;

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned to CameraFollow script!");
            enabled = false;
            return;
        }

        // Set initial rotation
        currentRotationY = angle;
        currentRotationX = 180f; // Start behind the player

        // Initialize the camera position
        UpdateCameraPosition();
        transform.position = desiredPosition;
        transform.rotation = desiredRotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * orbitSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * orbitSpeed;

        // Update rotation
        currentRotationX += mouseX;
        currentRotationY -= mouseY; // Inverted for intuitive control
        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);

        UpdateCameraPosition();

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
    }

    void UpdateCameraPosition()
    {
        // Calculate new position
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        desiredPosition = rotation * negDistance + target.position;

        // Adjust height
        desiredPosition.y = target.position.y + height;

        // Calculate rotation
        desiredRotation = Quaternion.LookRotation(target.position - desiredPosition, Vector3.up);
    }

    public Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0; // Project onto the horizontal plane
        return forward.normalized;
    }

    public Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right.y = 0; // Project onto the horizontal plane
        return right.normalized;
    }
}
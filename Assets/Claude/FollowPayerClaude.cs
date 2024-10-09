using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float height = 10f;
    public float distance = 5f;
    public float smoothSpeed = 0.125f;
    public float mouseSensitivity = 1f;
    public float maxPanDistance = 5f;
    public bool invertMouseX = true; // New variable to control Y-axis inversion
    public bool invertMouseY = true; // New variable to control Y-axis inversion

    private Vector3 currentVelocity;
    private Vector2 currentPan;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Get mouse input for panning
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Invert the mouseY if invertMouseY is true
        if (invertMouseY)
        {
            mouseY = -mouseY;
        }
         if (invertMouseX)
        {
            mouseX = -mouseX;
        }

        // Update current pan
        currentPan += new Vector2(mouseX, mouseY);
        
        // Limit pan distance
        currentPan = Vector2.ClampMagnitude(currentPan, maxPanDistance);

        // Calculate camera position
        Vector3 targetPosition = target.position;
        Vector3 cameraPosition = targetPosition + new Vector3(currentPan.x, height, currentPan.y - distance);

        // Smoothly move the camera
        transform.position = Vector3.SmoothDamp(transform.position, cameraPosition, ref currentVelocity, smoothSpeed);

        // Make the camera look at the player's position (not considering height)
        Vector3 lookAtPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        transform.LookAt(lookAtPosition);
    }
}
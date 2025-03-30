using UnityEngine;

public class TruckCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;        // The truck to follow
    [SerializeField] private float followDistance = 7f;   // Distance behind the truck
    [SerializeField] private float heightOffset = 3f;     // Height above the truck
    [SerializeField] private float lateralOffset = 0f;    // Sideways offset (if needed)

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;      // Lower = smoother but slower
    [SerializeField] private float rotationSmooth = 5f;   // How quickly camera rotates
    [SerializeField] private float lookAheadDistance = 2f; // How far ahead to look at

    [Header("Collision Settings")]
    [SerializeField] private bool avoidWalls = true;      // Enable wall avoidance
    [SerializeField] private float minDistance = 1f;      // Minimum distance from walls
    [SerializeField] private LayerMask collisionLayers;   // Layers to check for collisions

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private Quaternion desiredRotation;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Camera target not assigned! Please assign your truck to the target field.");
            enabled = false;
            return;
        }
    }

    private void LateUpdate()
    {
        // Calculate the desired position behind and above the truck
        Vector3 targetForward = target.forward;
        Vector3 targetRight = target.right;
        
        desiredPosition = target.position
            - (targetForward * followDistance)    // Move behind
            + (Vector3.up * heightOffset)         // Move up
            + (targetRight * lateralOffset);      // Move sideways
            
        // Calculate look-at position (slightly ahead of the truck)
        Vector3 lookAtPos = target.position + (target.forward * lookAheadDistance);

        // Handle wall avoidance
        if (avoidWalls)
        {
            RaycastHit hit;
            Vector3 directionToCamera = (desiredPosition - target.position).normalized;
            float targetDistance = Vector3.Distance(target.position, desiredPosition);

            if (Physics.Raycast(target.position, directionToCamera, out hit, targetDistance, collisionLayers))
            {
                // If there's a wall, position the camera at the hit point plus minimum distance
                desiredPosition = hit.point + (hit.normal * minDistance);
            }
        }

        // Smoothly move the camera
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Smoothly rotate to look at the target
        desiredRotation = Quaternion.LookRotation(lookAtPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSmooth * Time.deltaTime);
    }

    // Helper method to set a new target at runtime if needed
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Helper method to adjust follow distance at runtime
    public void SetFollowDistance(float distance)
    {
        followDistance = Mathf.Max(0.1f, distance); // Prevent negative or zero distance
    }
}
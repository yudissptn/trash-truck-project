using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxVelocity = 5f;
    public Camera playerCamera;

    private Rigidbody rb;
    private Vector3 movement;
    private CameraFollow cameraFollow;

    void Start()
    {
        Debug.Log("PlayerMovement Start method called");
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the player object.");
            enabled = false;
            return;
        }

        rb.freezeRotation = true;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("No camera assigned and no main camera found in the scene.");
                enabled = false;
                return;
            }
        }

        cameraFollow = playerCamera.GetComponent<CameraFollow>();
        if (cameraFollow == null)
        {
            Debug.LogError("CameraFollow script not found on the assigned camera.");
            enabled = false;
            return;
        }

        Debug.Log("PlayerMovement initialized successfully");
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = cameraFollow.GetCameraForward();
        Vector3 cameraRight = cameraFollow.GetCameraRight();

        movement = (cameraForward * moveVertical + cameraRight * moveHorizontal).normalized;
        
        Debug.Log($"Input: Horizontal = {moveHorizontal}, Vertical = {moveVertical}");
        Debug.Log($"Calculated movement: {movement}");
    }

    void FixedUpdate()
    {
        rb.AddForce(movement * moveSpeed);

        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxVelocity)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxVelocity;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, toRotation, 10f * Time.fixedDeltaTime));
        }

        Debug.Log($"Player velocity: {rb.velocity}");
    }
}
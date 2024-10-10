using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxVelocity = 5f;
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent the capsule from falling over
    }

    void Update()
    {
        // Get input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Calculate movement vector
        movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
    }

    void FixedUpdate()
    {
        // Apply force for movement
        rb.AddForce(movement * moveSpeed);

        // Limit velocity
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxVelocity)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxVelocity;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }

        // Rotate the player to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, toRotation, 10f * Time.fixedDeltaTime));
        }
    }
}
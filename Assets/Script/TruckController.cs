using UnityEngine;

public class TruckController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 50f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access the Rigidbody component on the truck
    }

    void Update()
    {
        // Get player input for moving forward/backward and turning
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // Move the truck forward/backward based on input
        rb.MovePosition(transform.position + transform.forward * move);

        // Rotate the truck left/right based on input
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
    }
}

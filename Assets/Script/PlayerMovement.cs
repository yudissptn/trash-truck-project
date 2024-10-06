using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Get input from A/D or Left/Right arrow keys
        float moveVertical = Input.GetAxis("Vertical"); // Get input from W/S or Up/Down arrow keys

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); // Create movement vector
        transform.position += movement * moveSpeed * Time.deltaTime; // Move the player
    }
}

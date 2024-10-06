using UnityEngine;

public class TrashCollector : MonoBehaviour
{
    public Transform hand; // Reference to the Hand object
    private GameObject trashInHand = null; // Stores the currently picked-up trash object
    public float pickupRange = 3f; // Range to pick up trash

    void Update()
    {
        // Check for pickup or drop input
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (trashInHand == null)
            {
                PickupTrash();
            }
            else
            {
                DropTrash();
            }
        }
    }

    void PickupTrash()
    {
        // Raycast to detect trash in front of the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                trashInHand = hit.collider.gameObject; // Store the trash object
                trashInHand.GetComponent<Rigidbody>().isKinematic = true; // Disable physics for the trash
                trashInHand.transform.position = hand.position; // Move trash to hand position
                trashInHand.transform.position = hand.position + new Vector3(0, 0.5f, 0); // Adjust this value as needed
                trashInHand.transform.SetParent(hand); // Parent to the hand
                Debug.Log("Picked up trash.");
            }
        }
    }

    void DropTrash()
    {
        if (trashInHand != null)
        {
            trashInHand.GetComponent<Rigidbody>().isKinematic = false; // Enable physics again
            trashInHand.transform.SetParent(null); // Unparent from the hand
            trashInHand = null; // Clear the reference
            Debug.Log("Dropped trash.");
        }
    }
}

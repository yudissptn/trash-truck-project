using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollector : MonoBehaviour
{
    public float pickupRange = 2f; // Range to detect trash
    public Transform hand; // Hand position to hold the trash
    public float throwMinForce = 5f; // Minimum force when throwing
    public float throwMaxForce = 15f; // Maximum force when throwing
    public float chargeSpeed = 10f; // Speed at which the throw force charges
    public Camera playerCamera; // Reference to the player's camera
    public Vector3 throwPositionOffset = new Vector3(0, 1.5f, 0); // Offset for the trash position while charging

    private GameObject trashInHand; // The trash object held by the player
    private bool isHoldingTrash = false; // To track if player is holding trash
    private bool isChargingThrow = false; // Track if player is charging throw
    private float currentThrowForce; // Store the throw force

    void Update()
    {
        // Pickup mechanic
        if (Input.GetKeyDown(KeyCode.E) && !isHoldingTrash)
        {
            PickupTrash(); // Pick up trash if not holding
        }

        // Charging and throwing mechanic when holding trash
        if (isHoldingTrash)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isChargingThrow) // Start charging when pressing Q
            {
                isChargingThrow = true;
                currentThrowForce = throwMinForce; // Reset throw force when starting to charge
            }

            if (isChargingThrow)
            {
                // Charge throw force while holding Q
                currentThrowForce = Mathf.Clamp(currentThrowForce + chargeSpeed * Time.deltaTime, throwMinForce, throwMaxForce);
                Debug.Log("Charging Throw: " + currentThrowForce);

                // Position the trash above the player
                trashInHand.transform.position = transform.position + throwPositionOffset;
            }

            if (Input.GetKeyUp(KeyCode.Q) && isChargingThrow) // Release Q to throw the trash
            {
                ThrowTrash(); // Throw the trash
            }
        }
    }

    void PickupTrash()
    {
        // Raycast to detect trash around the player
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 1.0f, transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                trashInHand = hit.collider.gameObject; // Store the trash object
                trashInHand.GetComponent<Rigidbody>().isKinematic = true; // Disable physics for the trash
                trashInHand.transform.position = hand.position; // Move trash to hand position
                trashInHand.transform.SetParent(hand); // Parent to the hand
                isHoldingTrash = true;
                Debug.Log("Picked up trash.");
            }
        }
    }

    void ThrowTrash()
    {
        if (trashInHand != null)
        {
            // Unparent the trash from the hand
            trashInHand.transform.SetParent(null);

            // Re-enable physics
            Rigidbody trashRigidbody = trashInHand.GetComponent<Rigidbody>();
            trashRigidbody.isKinematic = false;

            // Calculate the direction to throw based on mouse position
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 throwDirection;

            // Check if the ray hits anything (like the ground or objects)
            if (Physics.Raycast(ray, out hit))
            {
                throwDirection = (hit.point - hand.position).normalized;
            }
            else
            {
                // If no hit, throw in the forward direction as fallback
                throwDirection = transform.forward;
            }

            // Apply throw force in the direction of the mouse pointer
            trashRigidbody.AddForce(throwDirection * currentThrowForce, ForceMode.Impulse);

            // Reset values after throwing
            trashInHand = null;
            isHoldingTrash = false;
            isChargingThrow = false;
            currentThrowForce = throwMinForce;

            Debug.Log("Trash Thrown with force: " + currentThrowForce);
        }
    }
}



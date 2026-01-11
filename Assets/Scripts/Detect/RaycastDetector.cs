using UnityEngine;
using UnityEngine.UI;

public class RaycastDetector : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Transform referenceObject, camObj; // object from which the ray will start
    public float rayLength = 2f;

    [Header("Pickup Settings")]
    public Transform hand;           // where picked object will be held
    public Button interactButton;    // interact button

    private GameObject heldObject = null; // currently held object

    void Start()
    {
        if (interactButton != null)
            interactButton.onClick.AddListener(OnInteract);
    }

    void Update()
    {
        if (referenceObject == null || camObj == null) return;

        // Rotate referenceObject according to camObj (X and Y only)
        Vector3 targetEuler = camObj.eulerAngles;
        referenceObject.rotation = Quaternion.Euler(targetEuler.x, targetEuler.y, 0);

        // Shoot a ray forward from the reference object
        Ray ray = new Ray(referenceObject.position, referenceObject.forward);
        RaycastHit hit;

        // Debug ray in Scene view
        Debug.DrawRay(referenceObject.position, referenceObject.forward * rayLength, Color.green);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // Skip if hit object is the object this script is attached to
            if (hit.collider.gameObject == gameObject)
                return;

            // Print the tag of the object hit
            Debug.Log("Hit object: " + hit.collider.gameObject.name + " | Tag: " + hit.collider.gameObject.tag);
        }
    }

    void OnInteract()
    {
        // If already holding an object, drop it
        if (heldObject != null)
        {
            DropObject();
            return;
        }

        // Shoot ray to detect object in front
        Ray ray = new Ray(referenceObject.position, referenceObject.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            GameObject target = hit.collider.gameObject;

            // Skip if hit object is self
            if (target == gameObject)
                return;

            // Only pick up objects with tag "Pickup"
            if (target.CompareTag("Pickup"))
            {
                PickUp(target,0.1f);
            }
        }
    }

    void PickUp(GameObject obj,float reduceMultiplier)
    {
        heldObject = obj;

        // Parent it to the hand
        obj.transform.SetParent(hand);

        // Reset local position/rotation
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale *= reduceMultiplier;
        // Disable physics while holding
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        Debug.Log("Picked up: " + obj.name);
    }

    void DropObject()
    {
        if (heldObject == null) return;

        // Unparent the object
        heldObject.transform.SetParent(null);

        // Re-enable physics
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }

        Debug.Log("Dropped: " + heldObject.name);
        heldObject = null;
    }
}

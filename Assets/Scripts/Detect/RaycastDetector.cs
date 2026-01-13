using UnityEngine;
using UnityEngine.InputSystem.HID;
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
    public GameObject[] plantPrefab, cropPrefab;
    public float bigPlantHarvestRedius = 0.5f;
    private RaycastHit hit;
    private Ray ray;
    [Header("Indicator")]
    [Header("Indicator")]
    public GameObject indicatorPrefab;
    public Material validMaterial;   // green
    public Material invalidMaterial; // red

    private GameObject indicatorInstance;
    private Animator anim;
    void Start()
    {
        if (interactButton != null)
            interactButton.onClick.AddListener(OnInteract);
        if (indicatorPrefab != null)
        {
            indicatorInstance = Instantiate(indicatorPrefab);
            indicatorInstance.SetActive(false);
        }
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (referenceObject == null || camObj == null) return;

        // Rotate referenceObject according to camObj (X and Y only)
        Vector3 targetEuler = camObj.eulerAngles;
        referenceObject.rotation = Quaternion.Euler(targetEuler.x, targetEuler.y, 0);

        // Shoot a ray forward from the reference object
        ray = new Ray(referenceObject.position, referenceObject.forward);
       

        // Debug ray in Scene view
        Debug.DrawRay(referenceObject.position, referenceObject.forward * rayLength, Color.green);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            bool canPlace = false;
            // Skip if hit object is the object this script is attached to
            if (hit.collider.gameObject == gameObject)
                return;
            if (hit.collider.CompareTag("CropArea"))
            {
                Collider[] nearby = Physics.OverlapSphere(hit.point, 2f);
              //  canPlace = nearby.Length == 1; // example condition
                canPlace = true;
            }
            else
            {
                canPlace=false;
            }

            UpdateIndicator(hit.point, canPlace);
            // Print the tag of the object hit
            Debug.Log("Hit object: " + hit.collider.gameObject.name + " | Tag: " + hit.collider.gameObject.tag);
        }
        else
        {
            if (indicatorInstance != null)
                indicatorInstance.SetActive(false);
        }
    }
    void UpdateIndicator(Vector3 position, bool isValid)
    {
        if (indicatorInstance == null) return;

        indicatorInstance.SetActive(true);
        indicatorInstance.transform.position = position + Vector3.up * 0.02f;
        Debug.Log("Is valid placement: " + isValid);
        MeshRenderer mr = indicatorInstance.GetComponent<MeshRenderer>();
        mr.material = isValid ? validMaterial : invalidMaterial;
    }

    void OnInteract()
    {
        anim.Play("Interect");  
        // If already holding an object, drop it
        if (heldObject != null)
        {
            DropObject();
            return;
        }
        CharacterMovement.instance.FacePlayerToCamera();

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
        CharacterMovement.instance.FacePlayerToCamera();

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
        CharacterMovement.instance.FacePlayerToCamera();

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
    public void PlantingTree(string plantName)
    {
        if (referenceObject == null) return;
        CharacterMovement.instance.FacePlayerToCamera();
        Ray ray = new Ray(referenceObject.position, referenceObject.forward);
        RaycastHit hit;

        anim.Play("Interact");
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            GameObject target = hit.collider.gameObject;

            // Detect ONLY objects with tag "mytag"
            if (target.CompareTag("Land"))
            {
                // Instantiate demoObject at hit point
                for (int i = 0; i < plantPrefab.Length; i++)
                {
                    if(plantPrefab[i].name==plantName)
                    {
                        Collider[] nearby = Physics.OverlapSphere(hit.point, bigPlantHarvestRedius);

                        foreach (Collider col in nearby)
                        {
                            if (col.gameObject.name.Contains(plantPrefab[i].name))
                            {
                                Debug.Log("Harvest blocked: demoObject already nearby");
                                return; // ❌ Do not harvest
                            }
                        }
                        Instantiate(plantPrefab[i], hit.point, Quaternion.identity);
                        break;
                    }
                }

                Debug.Log("Harvested on: " + target.name);
            }
        }
    }
    public void plantingCrops(string cropName)
    {
        if (referenceObject == null) return;

        CharacterMovement.instance.FacePlayerToCamera();

        anim.Play("Interact");
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            GameObject target = hit.collider.gameObject;
            Debug.Log("Raycast hit: " + target.name);
            // Detect ONLY Fertilized Land
            if (!target.CompareTag("CropArea"))
                return;

            // Find CropArea around hit point
            Collider[] nearby = Physics.OverlapSphere(hit.point, bigPlantHarvestRedius);

            foreach (Collider col in nearby)
            {
                if (col.CompareTag("CropArea"))
                {
                    Debug.Log("Found CropArea: " + col.name);
                    // ❌ Prevent double planting
                    if (col.transform.childCount > 0)
                    {
                        Debug.Log("Crop already planted here");
                        return;
                    }
                    for (int i=0;i<cropPrefab.Length;i++)
                    {
                        if(cropPrefab[i].name==cropName)
                        {
                            Debug.Log("Crop already planted here");
                            // Instantiate crop at CropArea position
                            GameObject crop = Instantiate(
                                cropPrefab[i],
                                col.transform.position,
                                Quaternion.identity
                            );

                            // Parent crop to CropArea
                            crop.transform.SetParent(col.transform);

                            Debug.Log("Planted crop: " + cropName);
                            return;
                        }
                    }
                    
                    
                }
            }
        }
    }
               

}

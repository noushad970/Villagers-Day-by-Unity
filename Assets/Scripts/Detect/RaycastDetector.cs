using System.Collections.Generic;
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
    //public Button interactButton;    // interact button

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


    [SerializeField] private Button plantingButton, InterectButton;

    void Start()
    {
        if (InterectButton != null)
            InterectButton.onClick.AddListener(OnInteract);
        if (indicatorPrefab != null)
        {
            indicatorInstance = Instantiate(indicatorPrefab);
            indicatorInstance.SetActive(false);
        }
        anim = GetComponent<Animator>();
        if (treeSaveManager != null)
        {
            treeSaveManager.LoadTrees();
        }
        //ClearAllPlantedTrees();
        //treeSaveManager.WriteToFile();
        plantingButton.onClick.AddListener(() =>
                onClickPlantingButton(RemoveFirst7AndLast4(CharacterMovement.instance.handState.ToString())));

    }

    public string RemoveFirst7AndLast4(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Need at least 7 + 4 = 11 characters to remove both
        if (input.Length <= 11)
            return string.Empty;

        // Start after first 7, take length minus 7 (start) minus 4 (end)
        return input.Substring(7, input.Length - 11);
    }
    void Update()
    {
        detectWithRay();
    }
    private void onClickPlantingButton(string plantingObj)
    {
        plantingCrops(plantingObj);
        PlantingTree(plantingObj);
    }
    private void detectWithRay()
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
            //Debug.Log("Hit object: " + hit.collider.gameObject.name + " | Tag: " + hit.collider.gameObject.tag);
            if (hit.collider.gameObject == gameObject || hit.collider.gameObject.CompareTag("Tools"))
                return;
            if (hit.collider.CompareTag("CropArea"))
            {
                Collider[] nearby = Physics.OverlapSphere(hit.point, 2f);
                //  canPlace = nearby.Length == 1; // example condition

                canPlace = true;
            }
            else
            {
                canPlace = false;
            }
            if (hit.collider.CompareTag("Water"))
            {
                FishingManager.canFishing = true;
            }
            else
            {
                canPlace = false;
                FishingManager.canFishing = false;
            }
            UpdateIndicator(hit.point, canPlace);
            // Print the tag of the object hit
            // Debug.Log("Hit object: " + hit.collider.gameObject.name + " | Tag: " + hit.collider.gameObject.tag);
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
        if (!(CharacterMovement.instance.currentState.ToString() == "Idle") || (CharacterMovement.instance.handState.ToString() == "Empty")) return;
        indicatorInstance.SetActive(true);
        indicatorInstance.transform.position = position + Vector3.up * 0.02f;
      //  Debug.Log("Is valid placement: " + isValid);
        MeshRenderer mr = indicatorInstance.GetComponent<MeshRenderer>();
        mr.material = isValid ? validMaterial : invalidMaterial;
    }

    void OnInteract()
    {
        if (ActivateCraftingTool.Instance.isToolActive())
            return;
        anim.Play("Interect");  
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
            ShopkeeperInventory.instance.OpenShopUI(getObjectName());
            //collect crop
            if(target.CompareTag("CollectableCrop") && target.GetComponent<checkIsGrownCrop>().enabled==true)
            {
                Debug.Log("Collecting crop:" + target.name.ToString());
                //remove from save data
                GameObject pr = hit.collider.gameObject.transform.parent.gameObject;
                GameObject gPr = pr.transform.parent.gameObject;
                GameObject ggPr = gPr.transform.parent.gameObject;
                int index = hit.collider.gameObject.transform.GetSiblingIndex();
                Debug.Log("Collecting crop index:" + index.ToString());
                Debug.Log("Collecting crop parent:" + gPr.name.ToString());

                Debug.Log("Collecting crop grand parent:" + pr.name.ToString());
                ggPr.GetComponent<LandDataComponent>().RemoveCrop(target); 
                FarmController farmController = FindObjectOfType<FarmController>();
              //  Destroy(target);
                if (farmController != null)
                {
                    farmController.SaveFarm();
                    Debug.Log("Farm saved after sub-land activation.");

                }
                addCropToInventory(target.name.ToString());
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
    public TreeSaveManager treeSaveManager; // Assign in Inspector

    public void PlantingTree(string plantName)
    {
        if (referenceObject == null) return;
        Ray ray = new Ray(referenceObject.position, referenceObject.forward);
        RaycastHit hit;

        anim.Play("Interect");

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            GameObject target = hit.collider.gameObject;

            // Detect ONLY objects with tag "Land"
            if (!target.CompareTag("Land"))
                return;

            // Loop through plant prefabs
            for (int i = 0; i < plantPrefab.Length; i++)
            {
                if (plantPrefab[i].name == plantName)
                {
                    // Check if there's already a tree nearby
                    Collider[] nearby = Physics.OverlapSphere(hit.point, bigPlantHarvestRedius);
                    foreach (Collider col in nearby)
                    {
                        if (col.gameObject.name.Contains(plantPrefab[i].name))
                        {
                            Debug.Log("Harvest blocked: demoObject already nearby");
                            return; // ❌ Do not harvest
                        }
                    }

                    // Instantiate the tree
                    GameObject tree = Instantiate(plantPrefab[i], hit.point, Quaternion.identity);

                    // Save tree data with "Planted" state
                    if (treeSaveManager != null)
                    {
                        treeSaveManager.SaveTree(tree, "Planted");
                    }

                    Debug.Log($"Tree planted: {plantName} at {hit.point}");
                    break;
                }
            }
        }
    }
    public void RemoveTree(GameObject tree)
    {
        if (tree == null) return;

        // Update tree state in TreeSaveManager
        if (treeSaveManager != null)
        {
            treeSaveManager.ChangeTreeState(tree, "Cutted");
        }

        // Destroy the tree in the scene
        Destroy(tree);

        Debug.Log($"Tree removed: {tree.name}");
    }
    /// <summary>
    /// Deletes all planted trees from the TreeSaveManager database and destroys them in the scene
    /// </summary>
    public void ClearAllPlantedTrees()
    {
        if (treeSaveManager == null) return;
        List<GameObject> plantedTrees = new List<GameObject>(treeSaveManager.plantedTrees);
        foreach (GameObject tree in plantedTrees)
        {
            if (tree != null)
            {
                // Update tree state to "Cutted"
                treeSaveManager.ChangeTreeState(tree, "Cutted");
                // Destroy the tree in the scene
                Destroy(tree);
                Debug.Log($"Tree removed during clear: {tree.name}");
            }
        }
        // Clear the plantedTrees list in TreeSaveManager
        treeSaveManager.plantedTrees.Clear();
        Debug.Log("All planted trees have been cleared.");
    }

    public void plantingCrops(string cropName)
    {
        if (referenceObject == null) return;


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

                            //save to LandDataComponent
                            GameObject pr = hit.collider.gameObject.transform.parent.gameObject;
                            GameObject gPr = pr.transform.parent.gameObject;
                            int index = hit.collider.gameObject.transform.GetSiblingIndex();
                            gPr.GetComponent<LandDataComponent>().PlantCrop(index, cropName);
                            FarmController farmController = FindObjectOfType<FarmController>();
                            if (farmController != null)
                            {
                                farmController.SaveFarm();
                                Debug.Log("Farm saved after sub-land activation.");
                            }
                            return;
                        }
                    }
                    
                    
                }
            }
        }
    }
    //farmerShopKeeper,fisherShopKeeper,blacksmithShopKeeper,foodShopKeeper,meatShopKeeper,seedShopKeeper,animalShopKeeper
    public string getObjectName()
    {
       
        if (hit.collider.name.ToString()=="fisherShopKeeper" || hit.collider.name.ToString() == "foodShopKeeper" || hit.collider.name.ToString() == "farmerShopKeeper"|| hit.collider.name.ToString() =="blacksmithShopKeeper"|| hit.collider.name.ToString()== "meatShopKeeper"||hit.collider.name.ToString()== "seedShopKeeper" || hit.collider.name.ToString() == "animalShopKeeper")
        {
            Debug.Log("Collider Name: " + hit.collider.name);
            return hit.collider.name.ToString();
        }else
            return "";
    }
    public string RemoveCloneFromName(string fullName)
    {
        return fullName.Replace("(Clone)", "").Trim();
    }
    public void addCropToInventory(string cropName)
    {
        string getCropName= RemoveCloneFromName(cropName);
        PlayerSaveManager.Instance.AddPlantedOrCollectedItem(getCropName, 1);
    }
}

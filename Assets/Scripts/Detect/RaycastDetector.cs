using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        //ClearAllPlantedTrees();
        //treeSaveManager.WriteToFile();
        plantingButton.onClick.AddListener(() =>
                onClickPlantingButton(RemoveFirst7(CharacterMovement.instance.handState.ToString())));

    }

    public string RemoveLast4(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= 4)
            return string.Empty;

        return input.Substring(0, input.Length - 4);
    }
    public string RemoveFirst7(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= 7)
            return string.Empty;

        return input.Substring(7);
    }
    void Update()
    {
        detectWithRay();
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            InterectButton.onClick.Invoke();
        }
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            plantingButton.onClick.Invoke();
        }
    }
    private void onClickPlantingButton(string plantingObj)
    {
        if(plantingObj.Contains("Seed") || plantingObj.Contains("Tree"))
        {
            if (plantingObj.Contains("Seed"))
                plantingCrops(plantingObj);
            else
                PlantingTree(plantingObj);
        }

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
            Debug.Log("Can Fishing: " + FishingManager.canFishing);
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
        

        // Shoot ray to detect object in front
        Ray ray = new Ray(referenceObject.position, referenceObject.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            GameObject target = hit.collider.gameObject;

            // Skip if hit object is self
            if (target == gameObject)
                return;

            
            ShopkeeperInventory.instance.OpenShopUI(getObjectName());
            //collect crop
            if(target.CompareTag("CollectableCrop") && target.GetComponent<checkIsGrownCrop>().enabled==true)
            {
                NoticeUI.Instance.ShowNotice("Collected: " + RemoveCloneFromName(target.name.ToString()));
                //remove from save data
                GameObject pr = hit.collider.gameObject.transform.parent.gameObject;
                GameObject gPr = pr.transform.parent.gameObject;
                GameObject ggPr = gPr.transform.parent.gameObject;
                int index = hit.collider.gameObject.transform.GetSiblingIndex();
                ggPr.GetComponent<LandDataComponent>().RemoveCrop(target); 
                FarmController farmController = FindObjectOfType<FarmController>();
                AudioManager.Instance.playCollectSound();
              //  Destroy(target);
                if (farmController != null)
                {
                    farmController.SaveFarm();

                }
                addCropToInventory(target.name.ToString());
            }
            //water button detection and on off
            WateringSystem wat = target.GetComponent<WateringSystem>();
            if (target.CompareTag("MotorButton") && wat != null)
            {
                AudioManager.Instance.playClickSound();
                if (wat.IsWateringOn())
                {
                    wat.TurnOffWatering();
                }
                else
                {
                    wat.TurnOnWatering();
                }
            }
            //collect animal item
            if(target.GetComponent<AnimalLifeCycle>()!=null)
            {
                AnimalLifeCycle cycle = target.GetComponent<AnimalLifeCycle>();
                if (cycle.isReadyToCollect() && cycle != null)
                {
                    cycle.CollectItem();
                    AudioManager.Instance.playCollectSound();
                }
            }
            // collect wood
            if(target.CompareTag("Wood"))
            {
                PlayerSaveManager.Instance.AddItem("Wood", Random.Range(3,8));
                Destroy(target);
                AudioManager.Instance.playCollectSound();
                //adding particle of collection with sound
            }
            if (target.CompareTag("DoorNegative"))
            {
                DoorOpenClose door = target.GetComponent<DoorOpenClose>();
                AudioManager.Instance.playDoorSound();  
                door.doorOpenAndClose();
            }
            if (target.CompareTag("DeliveryMission"))
            {
                DeliveryPanelUI.instance.openDeliveryPanel();
                AudioManager.Instance.playClickSound();
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
        NoticeUI.Instance.ShowNotice("Picked up: " + obj.name);
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
        NoticeUI.Instance.ShowNotice("Dropped: " + heldObject.name);
        heldObject = null;
    }
    public TreeSaveManager treeSaveManager; // Assign in Inspector

    public void PlantingTree(string plantName)
    {
        if (referenceObject == null) return;
        Debug.Log("Player has " + PlayerSaveManager.Instance.GetItemCount(plantName) + " of " + plantName);
        if (PlayerSaveManager.Instance.GetItemCount(plantName)>0)
        {
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
                                NoticeUI.Instance.ShowNotice("Cannot plant here: " + plantName + " already nearby");
                                return; // ❌ Do not harvest
                            }
                        }

                        // Instantiate the tree
                        GameObject tree = Instantiate(plantPrefab[i], hit.point, Quaternion.identity);

                        // Save tree data with "Planted" state
                        if (treeSaveManager != null)
                        {
                            treeSaveManager.SaveTree(tree);
                            AudioManager.Instance.playPlantSound();
                            PlayerSaveManager.Instance.AddItem(plantName, -1);
                        }

                        NoticeUI.Instance.ShowNotice($"Planted: {plantName}");
                        break;
                    }
                }

            }
        }
        else
        {
            NoticeUI.Instance.ShowNotice("You don't have any " + plantName + " to plant.");
        }
    }

    public void RemoveTree(GameObject tree)
    {
        if (tree == null) return;

        // Update tree state in TreeSaveManager
        if (treeSaveManager != null)
        {
            treeSaveManager.ChangeTreeState(tree, "Cutted");
            Debug.Log("Tree Cutted");
        }

        // Destroy the tree in the scene
       // Destroy(tree);

        Debug.Log($"Tree removed: {tree.name}");
      //  NoticeUI.Instance.ShowNotice($"Removed: {tree.name}");
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
            }
        }
        // Clear the plantedTrees list in TreeSaveManager
        treeSaveManager.plantedTrees.Clear();
    }

    public void plantingCrops(string cropName)
    {
        if (referenceObject == null) return;

        if (PlayerSaveManager.Instance.GetItemCount(cropName) > 0)
        {
            anim.Play("Interect");
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

                        // ❌ Prevent double planting
                        if (col.transform.childCount > 0)
                        {
                            NoticeUI.Instance.ShowNotice("Cannot plant here: Crop already planted");
                            return;
                        }
                        string cropOr=RemoveLast4(cropName);
                        for (int i = 0; i < cropPrefab.Length; i++)
                        {
                            if (cropPrefab[i].name == cropOr)
                            {
                                // Instantiate crop at CropArea position
                                GameObject crop = Instantiate(
                                    cropPrefab[i],
                                    col.transform.position,
                                    Quaternion.identity
                                );

                                // Parent crop to CropArea
                                crop.transform.SetParent(col.transform);

                                NoticeUI.Instance.ShowNotice("Planted: " + cropOr);
                                AudioManager.Instance.playPlantSound();
                                //string seedname = cropName + "Seed";
                                PlayerSaveManager.Instance.AddItem(cropName, -1);
                                //save to LandDataComponent
                                GameObject pr = hit.collider.gameObject.transform.parent.gameObject;
                                GameObject gPr = pr.transform.parent.gameObject;
                                int index = hit.collider.gameObject.transform.GetSiblingIndex();
                                gPr.GetComponent<LandDataComponent>().PlantCrop(index, cropOr);
                                FarmController farmController = FindObjectOfType<FarmController>();
                                if (farmController != null)
                                {
                                    farmController.SaveFarm();
                                }
                                return;
                            }
                        }


                    }
                }
            }

        }
        else
        {
            NoticeUI.Instance.ShowNotice("You don't have any " + cropName + " to plant.");
        }
    }
    //farmerShopKeeper,fisherShopKeeper,blacksmithShopKeeper,foodShopKeeper,meatShopKeeper,seedShopKeeper,animalShopKeeper
    public string getObjectName()
    {
       
        if (hit.collider.name.ToString()=="fisherShopKeeper" || hit.collider.name.ToString() == "foodShopKeeper" || hit.collider.name.ToString() == "farmerShopKeeper"|| hit.collider.name.ToString() =="blacksmithShopKeeper"|| hit.collider.name.ToString()== "meatShopKeeper"||hit.collider.name.ToString()== "seedShopKeeper" || hit.collider.name.ToString() == "animalShopKeeper")
        {
            //hit.collider.gameObject.GetComponent<NPCShopman>().enableShopSection();
            EnableShop shopman = hit.collider.gameObject.GetComponent<EnableShop>();
           NoticeUI.Instance.ShowNotice("Shop Opened");
            shopman.enableShopSection();
            
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
        PlayerSaveManager.Instance.AddItem(getCropName, 1);
    }
}

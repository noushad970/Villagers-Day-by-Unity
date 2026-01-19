using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public static FishingManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isFishBited = false;
    public static bool canFishing=false;
    int randomBiteTime;
    [SerializeField] private GameObject fishingUI,congratsUI,SorryUI;
    [SerializeField] private TextMeshProUGUI helpingText;
    void Start()
    {
        Instance = this;
    }
    IEnumerator bitingFunction()
    {
        randomBiteTime = Random.Range(7, 16);
        isFishBited = false;
        yield return new WaitForSeconds(randomBiteTime);
        isFishBited = true;
        Debug.Log("Fish Bited");
        yield return new WaitForSeconds(1);
        Debug.Log("Fish Gone");
        isFishBited = false;
        helpingText.text = "Fish Gone! Try Again.";
        StartCoroutine(bitingFunction());

    }
    private void Update()
    {
        fishing();
        activateFishingUI(); 

    }
    private void fishing()
    {
        if (ActivateCraftingTool.isFishingRodeActive)
        {
            
            if (ActivateCraftingTool.fishRodPull)
            {
                ActivateCraftingTool.fishRodPull = false;
                StartCoroutine(wait5Sec());
                if (isFishBited)
                {
                    Debug.Log("You caught a fish!");
                    helpingText.text = "You caught a fish!";
                    congratsUI.SetActive(true);
                    SorryUI.SetActive(false);
                    // Add logic for catching a fish
                    StopCoroutine(bitingFunction());
                }
                else
                {
                    StopCoroutine(bitingFunction());
                    helpingText.text = "No fish caught. Try Again!";
                    SorryUI.SetActive(true);
                    congratsUI.SetActive(false);
                    Debug.Log("No fish caught.");
                    // Add logic for missing a fish
                }

            }
        }
        else
        {
                canFishing = false;
                fishingUI.SetActive(false);
                StopCoroutine(bitingFunction());
        }
    }
    IEnumerator wait5Sec()
    {
        
        yield return new WaitForSeconds(5);
        congratsUI.SetActive(false);
        SorryUI.SetActive(false);

    }
    private void activateFishingUI()
    {
        Debug.Log("Fishing UI Activated");
        Debug.Log("isFishingRodeActive:" + ActivateCraftingTool.isFishingRodeActive);
        Debug.Log("canFishing:" + canFishing);
        Debug.Log("fishingStage1:" + ActivateCraftingTool.fishingStage1);
        Debug.Log("fishingStage2:" + ActivateCraftingTool.fishingStage2);

        if (ActivateCraftingTool.isFishingRodeActive)
        {
            fishingUI.SetActive(true);
            if (!canFishing)
            {
                helpingText.text = "Search For a Fishing spot...";
            }
            else
            {
                
                if (ActivateCraftingTool.fishingStage1 && !ActivateCraftingTool.fishingStage2)
                {
                    helpingText.text = "Throw The rod to fishing...";
                }
                else if (ActivateCraftingTool.fishingStage2 && !ActivateCraftingTool.fishingStage1)
                {
                    helpingText.text = "Pull the rod up when a fish bites!";
                    if (isFishBited)
                    {
                        helpingText.text = "Fish Bited! Pull the rod!";
                    }
                }
            }
        }
    }
    public void bitingStart()
    {
        StartCoroutine(bitingFunction());
    }
    public void stopBiting()
    {
        StopCoroutine(bitingFunction());
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject[] inventorySlotsItems;
    [SerializeField] private TextMeshProUGUI[] CountSlotItemTexts;///11 for seed types
    private void Start()
    {
        for (int i = 0; i < inventorySlotsItems.Length; i++)
        {
            int index = i; // very important! capture current value

            inventorySlotsItems[i].GetComponent<Button>().onClick.AddListener(() =>
                OnChangingItemButtonClicked(inventorySlotsItems[index].GetComponent<Button>()));
        }
    }
    private void Update()
    {
        getSeedData();
    }
    public void getSeedData()
    {
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("BeanSeed") > 0)
        {
            inventorySlotsItems[0].SetActive(true);
            CountSlotItemTexts[0].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("BeanSeed").ToString();
        }
        else
        {
            inventorySlotsItems[0].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("BeetrootSeed") > 0)
        {
            inventorySlotsItems[1].SetActive(true);
            CountSlotItemTexts[1].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("BeetrootSeed").ToString();
        }
        else
        {
            inventorySlotsItems[1].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("BroccoliSeed") > 0)
        {
            inventorySlotsItems[2].SetActive(true);
            CountSlotItemTexts[2].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("BroccoliSeed").ToString();

        }
        else
        {
            inventorySlotsItems[2].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("CabbageSeed") > 0)
        {
            inventorySlotsItems[3].SetActive(true);
            CountSlotItemTexts[3].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("CabbageSeed").ToString();
        }
        else
        {
            inventorySlotsItems[3].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("CarrotSeed") > 0)
        {
            inventorySlotsItems[4].SetActive(true);
            CountSlotItemTexts[4].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("CarrotSeed").ToString();
        }
        else
        {
            inventorySlotsItems[4].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("ChilliSeed") > 0)
        {
            inventorySlotsItems[5].SetActive(true);
            CountSlotItemTexts[5].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("ChilliSeed").ToString();
        }
        else
        {
            inventorySlotsItems[5].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("CornSeed") > 0)
        {
            inventorySlotsItems[6].SetActive(true);
            CountSlotItemTexts[6].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("CornSeed").ToString();
        }
        else
        {
            inventorySlotsItems[6].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("PepperSeed") > 0)
        {
            inventorySlotsItems[7].SetActive(true);
            CountSlotItemTexts[7].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("PepperSeed").ToString();
        }
        else
        {
            inventorySlotsItems[7].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("PumkinSeed") > 0)
        {
            inventorySlotsItems[8].SetActive(true);
            CountSlotItemTexts[8].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("PumkinSeed").ToString();
        }
        else
        {
            inventorySlotsItems[8].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("TomatoSeed") > 0)
        {
            inventorySlotsItems[9].SetActive(true);
            CountSlotItemTexts[9].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("TomatoSeed").ToString();
        }
        else
        {
            inventorySlotsItems[9].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("WatermelonSeed") > 0)
        {
            inventorySlotsItems[10].SetActive(true);
            CountSlotItemTexts[10].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("WatermelonSeed").ToString();
        }
        else
        {
            inventorySlotsItems[10].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("WheatSeed") > 0)
        {
            inventorySlotsItems[11].SetActive(true);
            CountSlotItemTexts[11].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("WheatSeed").ToString();
        }
        else
        {
            inventorySlotsItems[11].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Bean") > 0)
        {
            inventorySlotsItems[12].SetActive(true);
            CountSlotItemTexts[12].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Bean").ToString();
        }
        else
        {
            inventorySlotsItems[12].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Beetroot") > 0)
        {
            inventorySlotsItems[13].SetActive(true);
            CountSlotItemTexts[13].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Beetroot").ToString();
        }
        else
        {
            inventorySlotsItems[13].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Broccoli") > 0)
        {
            inventorySlotsItems[14].SetActive(true);
            CountSlotItemTexts[14].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Broccoli").ToString();
        }
        else
        {
            inventorySlotsItems[14].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Cabbage") > 0)
        {
            inventorySlotsItems[15].SetActive(true);
            CountSlotItemTexts[15].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Cabbage").ToString();
        }
        else
        {
            inventorySlotsItems[15].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Carrot") > 0)
        {
            inventorySlotsItems[16].SetActive(true);
            CountSlotItemTexts[16].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Carrot").ToString();
        }
        else
        {
            inventorySlotsItems[16].SetActive(false);


        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Chilli") > 0)
        {
            inventorySlotsItems[17].SetActive(true);
            CountSlotItemTexts[17].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Chilli").ToString();
        }
        else
        {
            inventorySlotsItems[17].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Corn") > 0)
        {
            inventorySlotsItems[18].SetActive(true);
            CountSlotItemTexts[18].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Corn").ToString();
        }
        else
        {
            inventorySlotsItems[18].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Pepper") > 0)
        {
            inventorySlotsItems[19].SetActive(true);
            CountSlotItemTexts[19].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Pepper").ToString();
        }
        else
        {
            inventorySlotsItems[19].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Pumkin") > 0)
        {
            inventorySlotsItems[20].SetActive(true);
            CountSlotItemTexts[20].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Pumkin").ToString();
        }
        else
        {
            inventorySlotsItems[20].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Tomato") > 0)
        {
            inventorySlotsItems[21].SetActive(true);
            CountSlotItemTexts[21].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Tomato").ToString();
        }
        else
        {
            inventorySlotsItems[21].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Watermelon") > 0)
        {
            inventorySlotsItems[22].SetActive(true);
            CountSlotItemTexts[22].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Watermelon").ToString();
        }
        else
        {
            inventorySlotsItems[22].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Wheat") > 0)
        {
            inventorySlotsItems[23].SetActive(true);
            CountSlotItemTexts[23].text = PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount("Wheat").ToString();
        }
        else
        {
            inventorySlotsItems[23].SetActive(false);
        }

    }
    private void OnChangingItemButtonClicked(Button clickedButton)
    {
        string itemName="Holding"+ clickedButton.gameObject.name;
        CharacterMovement.instance.SetHandStateFromString(itemName);
        ActivateCraftingTool.Instance.setActiveAllToolsFalse();
        Debug.Log("Current state: " + CharacterMovement.instance.handState.ToString());
    }
    public void ChangeHandStatement()
    {

    }
}
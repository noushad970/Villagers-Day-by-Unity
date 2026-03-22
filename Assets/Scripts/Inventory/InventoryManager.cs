using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject[] inventorySlotsItems;
    [SerializeField] private TextMeshProUGUI[] CountSlotItemTexts;///11 for seed types

    //Rohu, Hilsa, Tilapia, Catfish, Salmon, Tuna, Mackerel, Sardine, Cod, Carp, Egg, Milk, Wool, Meat
    [SerializeField] private GameObject[] inHandObjects;
    public List<Button> toolsButton = new List<Button>();
    public List<GameObject> toolsUI = new List<GameObject>();
    private void Start()
    {
        for (int i = 0; i < inventorySlotsItems.Length; i++)
        {
            int index = i; // very important! capture current value

            inventorySlotsItems[i].GetComponent<Button>().onClick.AddListener(() =>
                OnChangingItemButtonClicked(inventorySlotsItems[index].GetComponent<Button>()));
        }
        for (int i = 0; i < inHandObjects.Length; i++)
        {
            inHandObjects[i].SetActive(false);
        }
        for (int i = 0; i < toolsUI.Count; i++)
        {
            toolsUI[i].SetActive(false);
        }
        for (int i = 0; i < toolsButton.Count; i++)
        {
            int index = i; // important for closure

            toolsButton[i].onClick.AddListener(() => toolsButtonPressed(index));
        }
    }
    void falseEveryUI()
    {
        for (int i = 0; i < inHandObjects.Length; i++)
        {
            inHandObjects[i].SetActive(false);
        }
        for (int i = 0; i < toolsUI.Count; i++)
        {
            toolsUI[i].SetActive(false);
        }
    }
    void toolsButtonPressed(int index)
    {
        falseEveryUI();
        for (int i = 0; i < toolsUI.Count; i++)
        {
            toolsUI[i].SetActive(i == index);
        }
    }
    
    private void Update()
    {
        getSeedData();
    }
    public void getSeedData()
    {
        if (PlayerSaveManager.Instance.GetItemCount("BeanSeed") > 0)
        {
            inventorySlotsItems[0].SetActive(true);
            CountSlotItemTexts[0].text = PlayerSaveManager.Instance.GetItemCount("BeanSeed").ToString();
        }
        else
        {
            inventorySlotsItems[0].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("BeetrootSeed") > 0)
        {
            inventorySlotsItems[1].SetActive(true);
            CountSlotItemTexts[1].text = PlayerSaveManager.Instance.GetItemCount("BeetrootSeed").ToString();
        }
        else
        {
            inventorySlotsItems[1].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("BroccoliSeed") > 0)
        {
            inventorySlotsItems[2].SetActive(true);
            CountSlotItemTexts[2].text = PlayerSaveManager.Instance.GetItemCount("BroccoliSeed").ToString();

        }
        else
        {
            inventorySlotsItems[2].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("CabbageSeed") > 0)
        {
            inventorySlotsItems[3].SetActive(true);
            CountSlotItemTexts[3].text = PlayerSaveManager.Instance.GetItemCount("CabbageSeed").ToString();
        }
        else
        {
            inventorySlotsItems[3].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("CarrotSeed") > 0)
        {
            inventorySlotsItems[4].SetActive(true);
            CountSlotItemTexts[4].text = PlayerSaveManager.Instance.GetItemCount("CarrotSeed").ToString();
        }
        else
        {
            inventorySlotsItems[4].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("ChilliSeed") > 0)
        {
            inventorySlotsItems[5].SetActive(true);
            CountSlotItemTexts[5].text = PlayerSaveManager.Instance.GetItemCount("ChilliSeed").ToString();
        }
        else
        {
            inventorySlotsItems[5].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("CornSeed") > 0)
        {
            inventorySlotsItems[6].SetActive(true);
            CountSlotItemTexts[6].text = PlayerSaveManager.Instance.GetItemCount("CornSeed").ToString();
        }
        else
        {
            inventorySlotsItems[6].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("PepperSeed") > 0)
        {
            inventorySlotsItems[7].SetActive(true);
            CountSlotItemTexts[7].text = PlayerSaveManager.Instance.GetItemCount("PepperSeed").ToString();
        }
        else
        {
            inventorySlotsItems[7].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("PumkinSeed") > 0)
        {
            inventorySlotsItems[8].SetActive(true);
            CountSlotItemTexts[8].text = PlayerSaveManager.Instance.GetItemCount("PumkinSeed").ToString();
        }
        else
        {
            inventorySlotsItems[8].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("TomatoSeed") > 0)
        {
            inventorySlotsItems[9].SetActive(true);
            CountSlotItemTexts[9].text = PlayerSaveManager.Instance.GetItemCount("TomatoSeed").ToString();
        }
        else
        {
            inventorySlotsItems[9].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("WatermelonSeed") > 0)
        {
            inventorySlotsItems[10].SetActive(true);
            CountSlotItemTexts[10].text = PlayerSaveManager.Instance.GetItemCount("WatermelonSeed").ToString();
        }
        else
        {
            inventorySlotsItems[10].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("WheatSeed") > 0)
        {
            inventorySlotsItems[11].SetActive(true);
            CountSlotItemTexts[11].text = PlayerSaveManager.Instance.GetItemCount("WheatSeed").ToString();
        }
        else
        {
            inventorySlotsItems[11].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Bean") > 0)
        {
            inventorySlotsItems[12].SetActive(true);
            CountSlotItemTexts[12].text = PlayerSaveManager.Instance.GetItemCount("Bean").ToString();
        }
        else
        {
            inventorySlotsItems[12].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Beetroot") > 0)
        {
            inventorySlotsItems[13].SetActive(true);
            CountSlotItemTexts[13].text = PlayerSaveManager.Instance.GetItemCount("Beetroot").ToString();
        }
        else
        {
            inventorySlotsItems[13].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Broccoli") > 0)
        {
            inventorySlotsItems[14].SetActive(true);
            CountSlotItemTexts[14].text = PlayerSaveManager.Instance.GetItemCount("Broccoli").ToString();
        }
        else
        {
            inventorySlotsItems[14].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Cabbage") > 0)
        {
            inventorySlotsItems[15].SetActive(true);
            CountSlotItemTexts[15].text = PlayerSaveManager.Instance.GetItemCount("Cabbage").ToString();
        }
        else
        {
            inventorySlotsItems[15].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Carrot") > 0)
        {
            inventorySlotsItems[16].SetActive(true);
            CountSlotItemTexts[16].text = PlayerSaveManager.Instance.GetItemCount("Carrot").ToString();
        }
        else
        {
            inventorySlotsItems[16].SetActive(false);


        }
        if (PlayerSaveManager.Instance.GetItemCount("Chilli") > 0)
        {
            inventorySlotsItems[17].SetActive(true);
            CountSlotItemTexts[17].text = PlayerSaveManager.Instance.GetItemCount("Chilli").ToString();
        }
        else
        {
            inventorySlotsItems[17].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Corn") > 0)
        {
            inventorySlotsItems[18].SetActive(true);
            CountSlotItemTexts[18].text = PlayerSaveManager.Instance.GetItemCount("Corn").ToString();
        }
        else
        {
            inventorySlotsItems[18].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Pepper") > 0)
        {
            inventorySlotsItems[19].SetActive(true);
            CountSlotItemTexts[19].text = PlayerSaveManager.Instance.GetItemCount("Pepper").ToString();
        }
        else
        {
            inventorySlotsItems[19].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Pumkin") > 0)
        {
            inventorySlotsItems[20].SetActive(true);
            CountSlotItemTexts[20].text = PlayerSaveManager.Instance.GetItemCount("Pumkin").ToString();
        }
        else
        {
            inventorySlotsItems[20].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Tomato") > 0)
        {
            inventorySlotsItems[21].SetActive(true);
            CountSlotItemTexts[21].text = PlayerSaveManager.Instance.GetItemCount("Tomato").ToString();
        }
        else
        {
            inventorySlotsItems[21].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Watermelon") > 0)
        {
            inventorySlotsItems[22].SetActive(true);
            CountSlotItemTexts[22].text = PlayerSaveManager.Instance.GetItemCount("Watermelon").ToString();
        }
        else
        {
            inventorySlotsItems[22].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Wheat") > 0)
        {
            inventorySlotsItems[23].SetActive(true);
            CountSlotItemTexts[23].text = PlayerSaveManager.Instance.GetItemCount("Wheat").ToString();
        }
        else
        {
            inventorySlotsItems[23].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Rohu") > 0)
        {
            inventorySlotsItems[24].SetActive(true);
            CountSlotItemTexts[24].text = PlayerSaveManager.Instance.GetItemCount("Rohu").ToString();
        }
        else
        {
            inventorySlotsItems[24].SetActive(false);
        }
        if(PlayerSaveManager.Instance.GetItemCount("Hilsa") > 0)
        {
            inventorySlotsItems[25].SetActive(true);
            CountSlotItemTexts[25].text = PlayerSaveManager.Instance.GetItemCount("Hilsa").ToString();
        }
        else
        {
            inventorySlotsItems[25].SetActive(false);
        }
         if(PlayerSaveManager.Instance.GetItemCount("Tilapia") > 0)
        {
            inventorySlotsItems[26].SetActive(true);
            CountSlotItemTexts[26].text = PlayerSaveManager.Instance.GetItemCount("Tilapia").ToString();
        }
        else
        {
            inventorySlotsItems[26].SetActive(false);
        }
         if(PlayerSaveManager.Instance.GetItemCount("Catfish") > 0)
        {
            inventorySlotsItems[27].SetActive(true);
            CountSlotItemTexts[27].text = PlayerSaveManager.Instance.GetItemCount("Catfish").ToString();
        }
        else
        {
            inventorySlotsItems[27].SetActive(false);
        }
         if(PlayerSaveManager.Instance.GetItemCount("Salmon") > 0)
        {
            inventorySlotsItems[28].SetActive(true);
            CountSlotItemTexts[28].text = PlayerSaveManager.Instance.GetItemCount("Salmon").ToString();
        }
        else
        {
            inventorySlotsItems[28].SetActive(false);
        }
         if(PlayerSaveManager.Instance.GetItemCount("Tuna") > 0)
        {
            inventorySlotsItems[29].SetActive(true);
            CountSlotItemTexts[29].text = PlayerSaveManager.Instance.GetItemCount("Tuna").ToString();
        }
        else
        {
            inventorySlotsItems[29].SetActive(false);
        }
         if(PlayerSaveManager.Instance.GetItemCount("Mackerel") > 0)
        {
            inventorySlotsItems[30].SetActive(true);
            CountSlotItemTexts[30].text = PlayerSaveManager.Instance.GetItemCount("Mackerel").ToString();
        }
        else
        {
            inventorySlotsItems[30].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Sardine") > 0)
        {
            inventorySlotsItems[31].SetActive(true);
            CountSlotItemTexts[31].text = PlayerSaveManager.Instance.GetItemCount("Sardine").ToString();
        }
        else {
            inventorySlotsItems[31].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Cod") > 0)
        {
            inventorySlotsItems[32].SetActive(true);
            CountSlotItemTexts[32].text = PlayerSaveManager.Instance.GetItemCount("Cod").ToString();
        }
        else
        {
            inventorySlotsItems[32].SetActive(false);
        }
        if(PlayerSaveManager.Instance.GetItemCount("Carp") > 0)
        {
            inventorySlotsItems[33].SetActive(true);
            CountSlotItemTexts[33].text = PlayerSaveManager.Instance.GetItemCount("Carp").ToString();
        }
        else
        {
            inventorySlotsItems[33].SetActive(false);
        }
        if(PlayerSaveManager.Instance.GetItemCount("Egg") > 0)
        {
            inventorySlotsItems[34].SetActive(true);
            CountSlotItemTexts[34].text = PlayerSaveManager.Instance.GetItemCount("Egg").ToString();
        }
        else
        {
            inventorySlotsItems[34].SetActive(false);
        }
            if(PlayerSaveManager.Instance.GetItemCount("Milk") > 0)
            {
                inventorySlotsItems[35].SetActive(true);
                CountSlotItemTexts[35].text = PlayerSaveManager.Instance.GetItemCount("Milk").ToString();
            }
            else
            {
                inventorySlotsItems[35].SetActive(false);
            }
            if(PlayerSaveManager.Instance.GetItemCount("Wool") > 0)
            {
                inventorySlotsItems[36].SetActive(true);
                CountSlotItemTexts[36].text = PlayerSaveManager.Instance.GetItemCount("Wool").ToString();
            }
            else
            {
                inventorySlotsItems[36].SetActive(false);
            }
            if(PlayerSaveManager.Instance.GetItemCount("Meat") > 0)
            {
                inventorySlotsItems[37].SetActive(true);
                CountSlotItemTexts[37].text = PlayerSaveManager.Instance.GetItemCount("Meat").ToString();
            }

            else
            {
                inventorySlotsItems[37].SetActive(false);
            }
        if (PlayerSaveManager.Instance.GetItemCount("Wood") > 0)
        {
            inventorySlotsItems[38].SetActive(true);
            CountSlotItemTexts[38].text = PlayerSaveManager.Instance.GetItemCount("Wood").ToString();
        }

        else
        {
            inventorySlotsItems[38].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Rice") > 0)
        {
            inventorySlotsItems[39].SetActive(true);
            CountSlotItemTexts[39].text = PlayerSaveManager.Instance.GetItemCount("Rice").ToString();
        }

        else
        {
            inventorySlotsItems[39].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Flour") > 0)
        {
            inventorySlotsItems[40].SetActive(true);
            CountSlotItemTexts[40].text = PlayerSaveManager.Instance.GetItemCount("Flour").ToString();
        }

        else
        {
            inventorySlotsItems[40].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("Suger") > 0)
        {
            inventorySlotsItems[41].SetActive(true);
            CountSlotItemTexts[41].text = PlayerSaveManager.Instance.GetItemCount("Suger").ToString();
        }

        else
        {
            inventorySlotsItems[41].SetActive(false);
        }
        if (PlayerSaveManager.Instance.GetItemCount("BigTree") > 0)
        {
            inventorySlotsItems[42].SetActive(true);
            CountSlotItemTexts[42].text = PlayerSaveManager.Instance.GetItemCount("BigTree").ToString();
        }

        else
        {
            inventorySlotsItems[42].SetActive(false);
        }

        if (PlayerSaveManager.Instance.GetItemCount("CrismasTree") > 0)
        {
            inventorySlotsItems[43].SetActive(true);
            CountSlotItemTexts[43].text = PlayerSaveManager.Instance.GetItemCount("CrismasTree").ToString();
        }

        else
        {
            inventorySlotsItems[43].SetActive(false);
        }



    }
    private void OnChangingItemButtonClicked(Button clickedButton)
    {
        falseEveryUI();
        string itemName="Holding"+ clickedButton.gameObject.name;
        Debug.Log("Clicked item: " + itemName);
        CharacterMovement.instance.SetHandStateFromString(itemName);
        string itime= clickedButton.gameObject.name;
        ActivateCraftingTool.Instance.setActiveAllToolsFalse();
        Debug.Log("Current state: " + CharacterMovement.instance.handState.ToString());
        for(int i=0; i<inHandObjects.Length; i++)
        {
            if(inHandObjects[i].name == itime)
            {
                inHandObjects[i].SetActive(true);
            }
            else
            {
                inHandObjects[i].SetActive(false);
            }
        }
    }
    public void ChangeHandStatement()
    {

    }
}
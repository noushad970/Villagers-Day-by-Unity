using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCShopman : MonoBehaviour
{
    //farmerShopKeeper,fisherShopKeeper,blacksmithShopKeeper,foodShopKeeper,meatShopKeeper,seedShopKeeper,animalShopKeeper
    [SerializeField] private string shopKeeperName;
    [SerializeField] private Button sellPanelButton,buyPanelButton;
    [SerializeField] private GameObject sellPanel,buyPanel;
    [SerializeField] private GameObject[] fishesSell,fishesBuy;//Rohu Hilsa (Ilish) Tilapia Catfish Salmon Tuna Mackerel Sardine Cod Carp fishFood
    [SerializeField] private GameObject[] foodItemSell,foodItemBuy;
    [SerializeField] private GameObject[] meatItemSell,meatItemBuy;
    [SerializeField] private GameObject[] seedItemSell,seedItemBuy;
    [SerializeField] private GameObject[] animalItemSell,animalItemBuy;
    [SerializeField] private GameObject[] blacksmithItemSell,blacksmithItemBuy;
    [SerializeField] private GameObject[] farmerItemSell,farmerItemBuy;

    [SerializeField] private Button yesButton,NoButton;

    private GameObject itms;
    private string selectedItemName;
    private int selectedItemPrice;

    private void Start()
    {
        shopKeeperName = gameObject.name;
        sellPanelButton.onClick.AddListener(onClickSellButton);
        buyPanelButton.onClick.AddListener(onClickBuyButton);
        sellPanel.SetActive(true);
        buyPanel.SetActive(false);
        yesButton.onClick.AddListener(confirmPanelYes);

        NoButton.onClick.AddListener(confirmPanelNo);
        ButtonUpdateBuyAndSell();

    }

    private void ButtonUpdateBuyAndSell()
    {
        for (int i = 0; i < fishesSell.Length; i++)
        {
            if (!fishesSell[i]) return;
            GameObject sellItem = fishesSell[i];
            sellItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(sellItem));
        }

        for (int i = 0; i < fishesBuy.Length; i++)
        {
            if(!fishesBuy[i]) return;
            GameObject buyItem = fishesBuy[i];
            buyItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < foodItemSell.Length; i++)
        {
            if(!foodItemSell[i]) return;
            GameObject sellItem = foodItemSell[i];
            sellItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < foodItemBuy.Length; i++)
        {
            if(!foodItemBuy[i]) return;
            GameObject buyItem = foodItemBuy[i];
            buyItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < meatItemSell.Length; i++)
        {
            if(!meatItemSell[i]) return;
            GameObject sellItem = meatItemSell[i];
            sellItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < meatItemBuy.Length; i++)
        {
            if(!meatItemBuy[i]) return;
            GameObject buyItem = meatItemBuy[i];
            buyItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < seedItemSell.Length; i++)
        {
            if(!seedItemSell[i]) return;
            GameObject sellItem = seedItemSell[i];
            sellItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < seedItemBuy.Length; i++)
        {
            if(!seedItemBuy[i]) return;
            GameObject buyItem = seedItemBuy[i];
            buyItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < animalItemSell.Length; i++)
        {
            if(!animalItemSell[i]) return;
            GameObject sellItem = animalItemSell[i];
            sellItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < animalItemBuy.Length; i++)
        {
            if(!animalItemBuy[i]) return;
            GameObject buyItem = animalItemBuy[i];
            buyItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < blacksmithItemSell.Length; i++)
        {
            if(!blacksmithItemSell[i]) return;
            GameObject sellItem = blacksmithItemSell[i];
            sellItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < blacksmithItemBuy.Length; i++)
        {
            if(!blacksmithItemBuy[i]) return;
            GameObject buyItem = blacksmithItemBuy[i];
            buyItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < farmerItemSell.Length; i++)
        {
            if(!farmerItemSell[i]) return;
            GameObject sellItem = farmerItemSell[i];
            sellItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < farmerItemBuy.Length; i++)
        {
            if(!farmerItemBuy[i]) return;
            GameObject buyItem = farmerItemBuy[i];
            buyItem.GetComponent<Button>()
                .onClick.AddListener(() => onClickShopItemButton(buyItem));
        }

    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(02f);
        PlayerSaveManager.Instance.AddPlantedOrCollectedItem("Rohu", 5);

    }
    public string ShopKeeperName { get { return shopKeeperName; } }
    private void onClickSellButton()
    {
        sellPanel.SetActive(true);
        buyPanel.SetActive(false);
    }
    private void onClickBuyButton()
    {
        sellPanel.SetActive(false);
        buyPanel.SetActive(true);
    }
    [SerializeField] private GameObject confirmPanel;
    public void onClickShopItemButton(GameObject itm)
    {
        itms = itm;
        selectedItemName = itm.name;
        selectedItemPrice = PlayerSaveManager.Instance.GetItemPrice(itm.name);
        if (sellPanel.activeSelf)
        {
            // Logic for selling the item to the shopkeeper
            if(PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount(itm.name.ToString())>0)
            {

                // Further logic for confirming the sale
                confirmPanel.SetActive(true);

               
            }
            else
            {
                Debug.Log("You don't have enough items to sell.");
            }
        }
        else if(buyPanel.activeSelf)
        {
            // Logic for buying the item from the shopkeeper
            if (PlayerSaveManager.Instance.GetItemPrice(itm.name.ToString()) <= PlayerSaveManager.Instance.GetCoinCount())
            {

                // Further logic for confirming the sale
                confirmPanel.SetActive(true);
            }
            else
            {
                Debug.Log("You don't have enough coins to buy.");
            }
        }
    }
    void confirmPanelYes()
    {
        if (string.IsNullOrEmpty(selectedItemName))
            return;

        if (sellPanel.activeSelf)
        {
            PlayerSaveManager.Instance.UsePlantedOrCollectedItem(selectedItemName, 1);
            PlayerSaveManager.Instance.AddCoins(selectedItemPrice);
        }
        else
        {
            PlayerSaveManager.Instance.AddPlantedOrCollectedItem(selectedItemName, 1);
            PlayerSaveManager.Instance.AddCoins(-selectedItemPrice);
        }

        confirmPanel.SetActive(false);
    }

    void confirmPanelNo()
    {
        confirmPanel.SetActive(false);
    }
}

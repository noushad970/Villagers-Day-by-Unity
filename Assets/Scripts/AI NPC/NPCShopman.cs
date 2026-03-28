using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCShopman : MonoBehaviour
{
    //farmerShopKeeper,fisherShopKeeper,blacksmithShopKeeper,foodShopKeeper,meatShopKeeper,seedShopKeeper,animalShopKeeper
    [SerializeField] private string shopKeeperName;
    [SerializeField] private Button sellPanelButton,buyPanelButton;
    [SerializeField] private GameObject sellPanel,buyPanel;
    [SerializeField] private Button[] fishesSell,fishesBuy;//Rohu Hilsa (Ilish) Tilapia Catfish Salmon Tuna Mackerel Sardine Cod Carp fishFood
    [SerializeField] private Button[] foodItemSell,foodItemBuy;
    [SerializeField] private Button[] meatItemSell,meatItemBuy;
    [SerializeField] private Button[] seedItemSell,seedItemBuy;
    [SerializeField] private Button[] animalItemSell,animalItemBuy;
    [SerializeField] private Button[] blacksmithItemSell,blacksmithItemBuy;
    [SerializeField] private Button[] farmerItemSell,farmerItemBuy;

    [SerializeField] private Button yesButton,NoButton;

    [SerializeField] private AnimalDeliveryMovementVehicle vehicle;
    private GameObject itms;
    private string selectedItemName;
    private int selectedItemPrice;

    private void Start()
    {
        shopKeeperName = gameObject.name;
        sellPanelButton.onClick.AddListener(onClickSellButton);
        buyPanelButton.onClick.AddListener(onClickBuyButton);
        //onClickBuyButton();
        yesButton.onClick.AddListener(confirmPanelYes);
       // onClickBuyButton();
        NoButton.onClick.AddListener(confirmPanelNo);
        ButtonUpdateBuyAndSell();

        //PlayerSaveManager.Instance.AddItem("Cow", -5);

    }
    private void OnEnable()
    {
        onClickBuyButton();
        isShopOpen = true;
    }
    private void OnDisable()
    {
        isShopOpen = false;
    }
    public void onClickCloseButton()
    {
        selectedItemName = "";
        selectedItemPrice = 0;
    }
    private void ButtonUpdateBuyAndSell()
    {
        for (int i = 0; i < fishesSell.Length; i++)
        {
            if (!fishesSell[i]) return;
            Button sellItem = fishesSell[i];
            sellItem.onClick.AddListener(() => onClickShopItemButton(sellItem));
        }

        for (int i = 0; i < fishesBuy.Length; i++)
        {
            if(!fishesBuy[i]) return;
            Button buyItem = fishesBuy[i];
            buyItem.onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < foodItemSell.Length; i++)
        {
            if(!foodItemSell[i]) return;
            Button sellItem = foodItemSell[i];
            sellItem.onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < foodItemBuy.Length; i++)
        {
            if(!foodItemBuy[i]) return;
            Button buyItem = foodItemBuy[i];
            buyItem.onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < meatItemSell.Length; i++)
        {
            if(!meatItemSell[i]) return;
            Button sellItem = meatItemSell[i];
            sellItem.onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < meatItemBuy.Length; i++)
        {
            if(!meatItemBuy[i]) return;
            Button buyItem = meatItemBuy[i];
            buyItem.onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < seedItemSell.Length; i++)
        {
            if(!seedItemSell[i]) return;
            Button sellItem = seedItemSell[i];
            sellItem.onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < seedItemBuy.Length; i++)
        {
            if(!seedItemBuy[i]) return;
            Button buyItem = seedItemBuy[i];
            buyItem.onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < animalItemSell.Length; i++)
        {
            if(!animalItemSell[i]) return;
            Button sellItem = animalItemSell[i];
            sellItem.onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < animalItemBuy.Length; i++)
        {
            
            if (!animalItemBuy[i]) return;
            Button buyItem = animalItemBuy[i];
            buyItem.onClick.AddListener(() => onClickShopItemButton(buyItem));
            

        }
        for (int i = 0; i < blacksmithItemSell.Length; i++)
        {
            if(!blacksmithItemSell[i]) return;
            Button sellItem = blacksmithItemSell[i];
            sellItem.onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < blacksmithItemBuy.Length; i++)
        {
            if(!blacksmithItemBuy[i]) return;
            Button buyItem = blacksmithItemBuy[i];
            buyItem.onClick.AddListener(() => onClickShopItemButton(buyItem));
        }
        for (int i = 0; i < farmerItemSell.Length; i++)
        {
            if(!farmerItemSell[i]) return;
            Button sellItem = farmerItemSell[i];
            sellItem.onClick.AddListener(() => onClickShopItemButton(sellItem));
        }
        for (int i = 0; i < farmerItemBuy.Length; i++)
        {
            if(!farmerItemBuy[i]) return;
            Button buyItem = farmerItemBuy[i];
            buyItem.onClick.AddListener(() => onClickShopItemButton(buyItem));
        }

    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(02f);
        PlayerSaveManager.Instance.AddItem("Rohu", 5);

    }
    public static bool isShopOpen = false;
    public string ShopKeeperName { get { return shopKeeperName; } }
    private void onClickSellButton()
    {
        AudioManager.Instance.playClickSound();
        sellPanel.SetActive(true);
        buyPanel.SetActive(false);
    }
    private void onClickBuyButton()
    {
        AudioManager.Instance.playClickSound();
        sellPanel.SetActive(false);
        buyPanel.SetActive(true);
    }
    [SerializeField] private GameObject confirmPanel;
    public void onClickShopItemButton(Button itm)
    {
        AudioManager.Instance.playClickSound();
        itms = itm.gameObject;
        selectedItemName = itm.name.ToString();
        selectedItemPrice = PlayerSaveManager.Instance.GetItemPrice(itm.name);
        if (sellPanel.activeSelf)
        {
            // Logic for selling the item to the shopkeeper
            if(PlayerSaveManager.Instance.GetItemCount(itm.name.ToString())>0)
            {

                // Further logic for confirming the sale
                confirmPanel.SetActive(true);


            }
            else
            {
                NoticeUI.Instance.ShowNotice("You don't have enough items to sell.");
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
                    NoticeUI.Instance.ShowNotice("You don't have enough coins to buy.");
            }
        }
    }
    void confirmPanelYes()
    {
        Debug.Log("Selected Item: " + selectedItemName + ", Price: " + selectedItemPrice);
        if (string.IsNullOrEmpty(selectedItemName))
        {
            return;

        }

        if (sellPanel.activeSelf)
        {
            PlayerSaveManager.Instance.AddItem(selectedItemName, -1);
            PlayerSaveManager.Instance.AddCoins(selectedItemPrice);
            AudioManager.Instance.playSellSound();
        }
        else
        {

            if ((selectedItemName == "Cow" || selectedItemName == "Sheep" || selectedItemName == "Sheep2" || selectedItemName == "Chicken" || selectedItemName == "Duck" || selectedItemName == "Goat1" || selectedItemName == "Goat2"))
            {
                if (AnimalDeliveryMovementVehicle.vehicleIsBusy)
                {
                    NoticeUI.Instance.ShowNotice("Vehicle is busy delivering another animal. Please wait.");

                }
                else
                {
                    if (PlayerSaveManager.Instance.GetItemCount(selectedItemName) <= 4)
                    {
                        vehicle.deliverAnimal(selectedItemName, true);
                        NoticeUI.Instance.ShowNotice("Delivering " + selectedItemName + " to your farm. Please wait...");
                        AudioManager.Instance.playBuySound();
                        PlayerSaveManager.Instance.AddItem(selectedItemName, 1);
                        PlayerSaveManager.Instance.AddCoins(-selectedItemPrice);
                    }
                    else
                    {
                        NoticeUI.Instance.ShowNotice("You have too many of this animal. Cannot deliver.");
                    }
                }
            }
            else
            {

                PlayerSaveManager.Instance.AddItem(selectedItemName, 1);
                PlayerSaveManager.Instance.AddCoins(-selectedItemPrice);
                AudioManager.Instance.playBuySound();

            }

           
        }

        confirmPanel.SetActive(false);
    }

    void confirmPanelNo()
    {
        confirmPanel.SetActive(false);
        AudioManager.Instance.playClickSound();
    }
}

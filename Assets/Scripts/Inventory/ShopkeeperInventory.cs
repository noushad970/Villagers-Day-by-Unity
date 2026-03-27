using UnityEngine;
using UnityEngine.UI;

public class ShopkeeperInventory : MonoBehaviour
{
    public GameObject farmerShopUI, fisherShopUI,
                      foodShopUI, meatShopUI, seedShopUI, animalShopUI,shopPanel;
    public Button closeButton;
    public static ShopkeeperInventory instance;
    public NPCShopman[] shopKeepers;
    private void Start()
    {
        instance=this;
        closeButton.onClick.AddListener(CloseAllShopUIMenuExit);
    }
    public void OpenShopUI(string shopKeeperName)
    {
        
        switch (shopKeeperName)
        {
            case "farmerShopKeeper":
                CloseAllShopUIs();
                shopPanel.SetActive(true);
                farmerShopUI.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "fisherShopKeeper":
                CloseAllShopUIs();
                shopPanel.SetActive(true);
                fisherShopUI.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
                
            case "foodShopKeeper":
                CloseAllShopUIs();
                foodShopUI.SetActive(true);
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "meatShopKeeper":
                CloseAllShopUIs();
                meatShopUI.SetActive(true);
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "seedShopKeeper":
                CloseAllShopUIs();
                seedShopUI.SetActive(true);
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "animalShopKeeper":
                CloseAllShopUIs();
                animalShopUI.SetActive(true);
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            default:
                Debug.LogWarning("Unknown shopkeeper: " + shopKeeperName);
                break;
        }
    }
    public void CloseAllShopUIs()
    {
        
        farmerShopUI.SetActive(false);
        shopPanel.SetActive(false);
        fisherShopUI.SetActive(false);
        foodShopUI.SetActive(false);
        meatShopUI.SetActive(false);
        seedShopUI.SetActive(false);
        animalShopUI.SetActive(false);
        closeButton.gameObject.SetActive(false); 
        
    }
    public void CloseAllShopUIMenuExit()
    {

        farmerShopUI.SetActive(false);
        shopPanel.SetActive(false);
        fisherShopUI.SetActive(false);
        foodShopUI.SetActive(false);
        meatShopUI.SetActive(false);
        seedShopUI.SetActive(false);
        animalShopUI.SetActive(false);
        closeButton.gameObject.SetActive(false);
        for (int i = 0; i < shopKeepers.Length; i++)
        {
            shopKeepers[i].onClickCloseButton();
        }
    }
}

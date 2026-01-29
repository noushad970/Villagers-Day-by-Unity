using UnityEngine;
using UnityEngine.UI;

public class ShopkeeperInventory : MonoBehaviour
{
    public GameObject farmerShopUI, fisherShopUI,
                      foodShopUI, meatShopUI, seedShopUI, animalShopUI,shopPanel;
    public Button closeButton;
    public static ShopkeeperInventory instance;
    private void Start()
    {
        instance=this;
        closeButton.onClick.AddListener(CloseAllShopUIs);
    }
    public void OpenShopUI(string shopKeeperName)
    {
        
        switch (shopKeeperName)
        {
            case "farmerShopKeeper":
                farmerShopUI.SetActive(true);
                CloseAllShopUIs();
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "fisherShopKeeper":
                fisherShopUI.SetActive(true);
                CloseAllShopUIs();
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
                
            case "foodShopKeeper":
                foodShopUI.SetActive(true);
                CloseAllShopUIs();
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "meatShopKeeper":
                meatShopUI.SetActive(true);
                CloseAllShopUIs();
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "seedShopKeeper":
                seedShopUI.SetActive(true);
                CloseAllShopUIs();
                shopPanel.SetActive(true);
                closeButton.gameObject.SetActive(true);
                break;
            case "animalShopKeeper":
                animalShopUI.SetActive(true);
                CloseAllShopUIs();
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
}

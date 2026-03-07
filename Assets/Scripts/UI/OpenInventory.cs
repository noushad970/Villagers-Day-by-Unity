using UnityEngine;
using UnityEngine.UI;

public class OpenInventory : MonoBehaviour
{
    public GameObject inventoryUI;
    public Button closeButton, InventoryButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        InventoryButton.onClick.AddListener(Open);
        closeButton.onClick.AddListener(Close);
    }

    // Update is called once per frame

    void Update()
    {
    }
    private void Open()
    {
        inventoryUI.SetActive(true);
    }
    private void Close() {
    
        inventoryUI.SetActive(false);
    }
}

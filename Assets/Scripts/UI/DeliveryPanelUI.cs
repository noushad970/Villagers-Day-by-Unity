using UnityEngine;

public class DeliveryPanelUI : MonoBehaviour
{
    public GameObject deliveryPanel;
    public static DeliveryPanelUI instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public void openDeliveryPanel()
    {
                deliveryPanel.SetActive(true);
    }
}

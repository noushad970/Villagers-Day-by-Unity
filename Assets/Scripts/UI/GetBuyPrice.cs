using TMPro;
using UnityEngine;

public class GetBuyPrice : MonoBehaviour
{
    private GameObject parentObj;
    private TextMeshProUGUI priceText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentObj = transform.parent.gameObject;

        priceText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        priceText.text = "BUY: " + PlayerSaveManager.Instance.GetItemPrice(parentObj.name.ToString()).ToString();
    }
}

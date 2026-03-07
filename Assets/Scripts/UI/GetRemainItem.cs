using TMPro;
using UnityEngine;

public class GetRemainItem : MonoBehaviour
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
        priceText.text = PlayerSaveManager.Instance.GetItemCount(parentObj.name.ToString()).ToString();
    }
}

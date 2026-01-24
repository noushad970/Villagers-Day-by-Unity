using TMPro;
using UnityEngine;

public class ItemCounr : MonoBehaviour
{
    private TextMeshProUGUI count;
    void Start()
    {
        count = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        count.text=PlayerSaveManager.Instance.GetPlantedOrCollectedItemCount(gameObject.name.ToString()).ToString();
    }
}

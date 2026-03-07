using TMPro;
using UnityEngine;

public class CoinShow : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private void Start()
    {
       // NoticeUI.Instance.ShowNotice("Welcome to the shop! You have " + PlayerSaveManager.Instance.GetCoinCount().ToString() + " coins.");
    }
    private void Update()
    {
        coinText.text=PlayerSaveManager.Instance.GetCoinCount().ToString();
    }
}

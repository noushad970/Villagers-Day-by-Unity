using System.Collections;
using TMPro;
using UnityEngine;

public class NoticeUI : MonoBehaviour
{
    public TextMeshProUGUI NoticeText;
    public GameObject noticeSection;
    public static NoticeUI Instance;
    private void Start()
    {
        noticeSection.SetActive(false);
        Instance = this;
    }
    public void ShowNotice(string message)
    {
        noticeSection.SetActive(true);
        NoticeText.text = message;
        StartCoroutine(hideNotice());
    }
    IEnumerator hideNotice()
    {
        yield return new WaitForSeconds(4f);
        noticeSection.SetActive(false);
    }
}

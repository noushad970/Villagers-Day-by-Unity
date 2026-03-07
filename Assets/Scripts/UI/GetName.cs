using TMPro;
using UnityEngine;

public class GetName : MonoBehaviour
{
    private GameObject parentObj;
    private TextMeshProUGUI nameText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
            parentObj= transform.parent.gameObject;
        
        nameText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = parentObj.name.ToString();
    }
}

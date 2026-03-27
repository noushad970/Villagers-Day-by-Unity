using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    public GameObject howToPlaySection;
    public Button backButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        howToPlaySection.SetActive(false);
        backButton.onClick.AddListener(onClickBackButton);
    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.cKey.isPressed )
            howToPlaySection.SetActive(true);

    }
    public void onClickBackButton()
    {
        howToPlaySection.SetActive(false);
    }
}

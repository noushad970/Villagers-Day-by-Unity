using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuUIControl : MonoBehaviour
{
    public Button pauseButton, resumeButton, quitButton, gameControlButton,soundOnOffButton,backfromTutorialButton;
    public GameObject menuObj,gameControlDetailUI;
    public TMP_Text soundStatusState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseButton.onClick.AddListener(onClickPauseButton);
        resumeButton.onClick.AddListener(onClickResumeButton);
        quitButton.onClick.AddListener(onClickQuitButton);
        soundOnOffButton.onClick.AddListener(onClickSoundOnOffButton);
        backfromTutorialButton.onClick.AddListener(onClickbackFromTutorial);
        gameControlButton.onClick.AddListener(onClickGameControlButton);

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            pauseButton.onClick.Invoke();

        }
    }
    void onClickPauseButton()
    {
        menuObj.SetActive(true);
    }
    void onClickResumeButton()
    {
        menuObj.SetActive(false);
    }
    void onClickQuitButton()
    {
        Application.Quit();
    }
    void onClickSoundOnOffButton()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("Sound", 0);
            soundStatusState.text = "SOUND [OFF]";
        }
        else
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("Sound", 1);
            soundStatusState.text = "SOUND [ON]";
        }

    }
    void onClickbackFromTutorial()
    {
        gameControlDetailUI.SetActive(false);
    }
    void onClickGameControlButton()
    {
        gameControlDetailUI.SetActive(true);
    }
}

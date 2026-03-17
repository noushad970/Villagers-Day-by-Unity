using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIControl : MonoBehaviour
{
    public Button pauseButton, resumeButton, quitButton, soundOnOffButton;
    public GameObject menuObj;
    public TMP_Text soundStatusState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseButton.onClick.AddListener(onClickPauseButton);
        resumeButton.onClick.AddListener(onClickResumeButton);
        quitButton.onClick.AddListener(onClickQuitButton);
        soundOnOffButton.onClick.AddListener(onClickSoundOnOffButton);

    }

    // Update is called once per frame
    void Update()
    {

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
}

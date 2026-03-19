using UnityEngine;

public class WateringSystem : MonoBehaviour
{
    public GameObject[] waterParticles;
    private bool isWateringOn = false;
    private void Start()
    {
        isWateringOn=false;
        for (int i = 0; i < waterParticles.Length; i++)
        {

            waterParticles[i].SetActive(false);

        }
    }
    
    public void TurnOnWatering()
    {
        AudioManager.Instance.playwateringSound();
        NoticeUI.Instance.ShowNotice("Watering On");
        Debug.Log("Watering On");
        Debug.Log("Game object name: " + this.gameObject.name);
        for (int i = 0; i < waterParticles.Length; i++)
        {
            waterParticles[i].SetActive(true);

        }
    }
    public void TurnOffWatering()
    {
        AudioManager.Instance.stopWateringSound();
        NoticeUI.Instance.ShowNotice("Watering Off");
        Debug.Log("Watering Off");
        for (int i = 0; i < waterParticles.Length; i++)
        {

            waterParticles[i].SetActive(false);

        }
    }
    
}

using UnityEngine;

public class WateringSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject onMotor, offMotor;
    public ParticleSystem[] waterParticles;
    private bool isWateringOn = false;
    private void Start()
    {
        isWateringOn=false;
        TurnOnWatering();

    }
    private void Update()
    {
        if(isWateringOn)
        {

            for(int i=0; i< waterParticles.Length; i++)
            {
                 waterParticles[i].gameObject.SetActive(true);
                
            }
        }
        else
        {
           for (int i = 0; i < waterParticles.Length; i++)
            {
                
                    waterParticles[i].gameObject.SetActive(false);
                
            }
        }
    }
    public void TurnOnWatering()
    {
        onMotor.SetActive(true);
        offMotor.SetActive(false);
        AudioManager.Instance.playwateringSound();
        isWateringOn = true;
        NoticeUI.Instance.ShowNotice("Watering On");
    }
    public void TurnOffWatering()
    {
        onMotor.SetActive(false);
        offMotor.SetActive(true);
        isWateringOn = false;
        AudioManager.Instance.stopWateringSound();
        NoticeUI.Instance.ShowNotice("Watering Off");
    }
    public bool IsWateringOn()
    {
        return isWateringOn;
    }
}

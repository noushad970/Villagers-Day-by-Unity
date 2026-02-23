using UnityEngine;

public class WateringSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject onMotor, offMotor;
    public ParticleSystem[] waterParticles;
    private bool isWateringOn = false;
    private void Start()
    {
        TurnOffWatering();
        isWateringOn=false;

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
        isWateringOn = true;
    }
    public void TurnOffWatering()
    {
        onMotor.SetActive(false);
        offMotor.SetActive(true);
        isWateringOn = false;
    }
    public bool IsWateringOn()
    {
        return isWateringOn;
    }
}

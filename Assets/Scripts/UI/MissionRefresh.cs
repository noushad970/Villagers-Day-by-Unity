using UnityEngine;

public class MissionRefresh : MonoBehaviour
{
    public MissionUI missionUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        missionUI.refreshMission();
    }
}

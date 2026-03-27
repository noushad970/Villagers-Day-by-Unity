using UnityEngine;

public class LocalFarmSaveManager : MonoBehaviour
{
    private const string FARM_KEY = "farm_data";

    
    // SAVE
    public void SaveFarm(FarmData farm)
    {
        string json = JsonUtility.ToJson(farm);
        PlayerPrefs.SetString(FARM_KEY, json);
        PlayerPrefs.Save();

    }

    // LOAD
    public FarmData LoadFarm()
    {
        if (!PlayerPrefs.HasKey(FARM_KEY))
            return null;

        string json = PlayerPrefs.GetString(FARM_KEY);
        FarmData farm = JsonUtility.FromJson<FarmData>(json);


        return farm;
    }

    public bool HasSave()
    {
        return PlayerPrefs.HasKey(FARM_KEY);
    }
}

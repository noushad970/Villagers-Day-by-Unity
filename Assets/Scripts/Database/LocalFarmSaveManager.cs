using UnityEngine;

public class LocalFarmSaveManager : MonoBehaviour
{
    private const string FARM_KEY = "farm_data";

    public void SaveFarm(FarmData data)
    {
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(FARM_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("✅ Farm data saved (PlayerPrefs)");
    }

    public FarmData LoadFarm()
    {
        if (!PlayerPrefs.HasKey(FARM_KEY))
        {
            Debug.LogWarning("No farm save found!");
            return null;
        }

        string json = PlayerPrefs.GetString(FARM_KEY);
        FarmData data = JsonUtility.FromJson<FarmData>(json);

        Debug.Log("✅ Farm data loaded (PlayerPrefs)");
        return data;
    }

    public bool HasSave()
    {
        return PlayerPrefs.HasKey(FARM_KEY);
    }
}
using UnityEngine;
using System.IO;

public class LocalFarmSaveManager : MonoBehaviour
{
    private string saveFileName = "FarmData.json";
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    public void SaveFarm(FarmData data)
    {
        string json = JsonUtility.ToJson(data, true); // pretty print
        File.WriteAllText(SavePath, json);
        Debug.Log("✅ Farm data saved at: " + SavePath);
    }

    public FarmData LoadFarm()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No farm save found!");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        FarmData data = JsonUtility.FromJson<FarmData>(json);
        Debug.Log("✅ Farm data loaded from: " + SavePath);
        return data;
    }

    public bool HasSave() => File.Exists(SavePath);
}

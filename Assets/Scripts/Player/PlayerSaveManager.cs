using UnityEngine;
using System.IO;

public class PlayerSaveManager : MonoBehaviour
{
    public static PlayerSaveManager Instance;

    private string savePath;
    public PlayerStateData playerData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Path.Combine(Application.persistentDataPath, "player_state.json");
            LoadPlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------- SAVE ----------------
    public void SavePlayer()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Player data saved");
    }

    // ---------------- LOAD ----------------
    public void LoadPlayer()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            playerData = JsonUtility.FromJson<PlayerStateData>(json);
            Debug.Log("Player data loaded");
        }
        else
        {
            playerData = new PlayerStateData();
            SavePlayer();
            Debug.Log("New player data created");
        }
    }

    // ---------------- COINS ----------------
    public void AddCoins(int amount)
    {
        playerData.coins += amount;
        SavePlayer();
    }

    public bool SpendCoins(int amount)
    {
        if (playerData.coins < amount) return false;
        playerData.coins -= amount;
        SavePlayer();
        return true;
    }

    // ---------------- SEEDS ----------------
    public void AddPlantedOrCollectedItem(string PlantedOrCollectedItemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(PlantedOrCollectedItemName);
        if (field != null)
        {
            int current = (int)field.GetValue(playerData);
            field.SetValue(playerData, current + amount);
            SavePlayer();
        }
        else
        {
            Debug.LogError("Seed not found: " + PlantedOrCollectedItemName);
        }
    }

    public bool UsePlantedOrCollectedItem(string PlantedOrCollectedItemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(PlantedOrCollectedItemName);
        if (field == null) return false;

        int current = (int)field.GetValue(playerData);
        if (current < amount) return false;

        field.SetValue(playerData, current - amount);
        SavePlayer();
        return true;
    }

    public int GetPlantedOrCollectedItemCount(string seedName)
    {
        var field = typeof(PlayerStateData).GetField(seedName);
        if (field == null) return 0;
        return (int)field.GetValue(playerData);
    }
}

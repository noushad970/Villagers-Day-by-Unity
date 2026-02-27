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
    public int GetCoinCount()
    {
        return playerData.coins;
    }
    // ---------------- SEEDS ----------------
    public void AddItem(string PlantedOrCollectedItemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(PlantedOrCollectedItemName);
        if (field != null)
        {
            int current = (int)field.GetValue(playerData);
            field.SetValue(playerData, current + amount);
            Debug.Log($"Added {amount} to {PlantedOrCollectedItemName}. New total: {(int)field.GetValue(playerData)}");
            SavePlayer();
        }
        else
        {
            Debug.LogError("Seed not found: " + PlantedOrCollectedItemName);
        }
    }
    public void UpdateItem(string PlantedOrCollectedItemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(PlantedOrCollectedItemName);
        if (field != null)
        {
            int current = (int)field.GetValue(playerData);
            field.SetValue(playerData, amount);
            Debug.Log($"Updated {amount} to {PlantedOrCollectedItemName}. New Update Value: {(int)field.GetValue(playerData)}");
            SavePlayer();
        }
        else
        {
            Debug.LogError("Seed not found: " + PlantedOrCollectedItemName);
        }
    }

    public bool UseItem(string PlantedOrCollectedItemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(PlantedOrCollectedItemName);
        if (field == null) return false;

        int current = (int)field.GetValue(playerData);
        if (current < amount) return false;

        field.SetValue(playerData, current - amount);
        SavePlayer();
        return true;
    }

    public int GetItemCount(string seedName)
    {
        var field = typeof(PlayerStateData).GetField(seedName);
        if (field == null) return 0;
        return (int)field.GetValue(playerData);
    }
    public int GetItemPrice(string itemName)
    {
        switch (itemName)
        {
            // Seeds
            case "BeanSeed": return 5;
            case "BeetrootSeed": return 6;
            case "BroccoliSeed": return 7;
            case "CabbageSeed": return 6;
            case "CarrotSeed": return 5;
            case "ChilliSeed": return 8;
            case "CornSeed": return 7;
            case "PepperSeed": return 8;
            case "PumkinSeed": return 10;
            case "TomatoSeed": return 6;
            case "WatermelonSeed": return 12;
            case "WheatSeed": return 4;

            // Crops
            case "Bean": return 20;
            case "Beetroot": return 25;
            case "Broccoli": return 30;
            case "Cabbage": return 22;
            case "Carrot": return 20;
            case "Chilli": return 35;
            case "Corn": return 28;
            case "Pepper": return 32;
            case "Pumkin": return 50;
            case "Tomato": return 24;
            case "Watermelon": return 60;
            case "Wheat": return 18;

            // Fish
            case "Rohu": return 80;
            case "Hilsa": return 150;
            case "Tilapia": return 70;
            case "Catfish": return 65;
            case "Salmon": return 200;
            case "Tuna": return 180;
            case "Mackerel": return 90;
            case "Sardine": return 50;
            case "Cod": return 100;   // ✅ example you asked for
            case "Carp": return 75;

            // animal items
            case "Egg": return 10;
            case "Milk": return 25;
            case "Wool": return 30;
            case "Meat": return 200;
            case "Wood": return 15;

            //food items
            case "Rice": return 40;
            case "Flour": return 30;
            case "Suger": return 40;



            // animals
            case "Cow": return 1000;
            case "Chicken": return 100;
            case "Sheep": return 500;
            case "Goat1": return 600;
            case "Goat2": return 600;
            case "Duck": return 100;

            default:
                Debug.LogWarning("Item price not found: " + itemName);
                return 0;
        }
    }

}

using UnityEngine;

public class PlayerSaveManager : MonoBehaviour
{
    public static PlayerSaveManager Instance;

    private const string SAVE_KEY = "player_state"; // instead of file path
    public PlayerStateData playerData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

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
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Player data saved (PlayerPrefs)");
    }

    // ---------------- LOAD ----------------
    public void LoadPlayer()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            playerData = JsonUtility.FromJson<PlayerStateData>(json);
            Debug.Log("Player data loaded (PlayerPrefs)");
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

    // ---------------- ITEMS ----------------
    public void AddItem(string itemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(itemName);
        if (field != null)
        {
            int current = (int)field.GetValue(playerData);
            field.SetValue(playerData, current + amount);
            SavePlayer();
        }
        else
        {
            Debug.LogError("Item not found: " + itemName);
        }
    }

    public void UpdateItem(string itemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(itemName);
        if (field != null)
        {
            field.SetValue(playerData, amount);
            SavePlayer();
        }
        else
        {
            Debug.LogError("Item not found: " + itemName);
        }
    }

    public bool UseItem(string itemName, int amount)
    {
        var field = typeof(PlayerStateData).GetField(itemName);
        if (field == null) return false;

        int current = (int)field.GetValue(playerData);
        if (current < amount) return false;

        field.SetValue(playerData, current - amount);
        SavePlayer();
        return true;
    }

    public int GetItemCount(string itemName)
    {
        var field = typeof(PlayerStateData).GetField(itemName);
        if (field == null) return 0;
        return (int)field.GetValue(playerData);
    }

    // PRICE FUNCTION (UNCHANGED)
    public int GetItemPrice(string itemName)
    {
        switch (itemName)
        {
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

            case "Rohu": return 80;
            case "Hilsa": return 150;
            case "Tilapia": return 70;
            case "Catfish": return 65;
            case "Salmon": return 200;
            case "Tuna": return 180;
            case "Mackerel": return 90;
            case "Sardine": return 50;
            case "Cod": return 100;
            case "Carp": return 75;

            case "Egg": return 10;
            case "Milk": return 25;
            case "Wool": return 30;
            case "Meat": return 200;
            case "Wood": return 15;

            case "Rice": return 40;
            case "Flour": return 30;
            case "Suger": return 40;

            case "Cow": return 1000;
            case "Chicken": return 100;
            case "Sheep": return 500;
            case "Goat1": return 600;
            case "Goat2": return 600;
            case "Duck": return 100;

            case "CrismasTree": return 50;
            case "BigTree": return 40;

            default:
                Debug.LogWarning("Item price not found: " + itemName);
                return 0;
        }
    }
}
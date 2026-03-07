using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public Transform spawnPointCow, spawnPointSheep, spawnPointChicken,spawnPointDuck, spawnPointGoat1,spawnPointGoat2, spawnPointSheep2;
    public GameObject cowPrefab, sheepPrefab, sheepPrefab2, chickenPrefab,duckPrefab,goatPrefab1,goatPrefab2;
    public Transform parentCow, parentSheep, parentSheep2, parentChicken,parentDuck,parentGoat1,parentGoat2;
    public static AnimalSpawner instance;
    private void Start()
    {
        instance=this;
        initializeAllAnimal();
       // PlayerSaveManager.Instance.AddCoins(22220); // Ensure coins are loaded and saved immediately


    }
    public void SpawnAnimal(string animalType)
    {
        switch (animalType)
        {
            case "Cow":
                Instantiate(cowPrefab, spawnPointCow.position, spawnPointCow.rotation, parentCow);
                break;
            case "Sheep":
                Instantiate(sheepPrefab, spawnPointSheep.position, spawnPointSheep.rotation,parentSheep);
                break;
            case "Sheep2":
                Instantiate(sheepPrefab2, spawnPointSheep2.position, spawnPointSheep2.rotation, parentSheep2);
                break;
            case "Chicken":
                Instantiate(chickenPrefab, spawnPointChicken.position, spawnPointChicken.rotation,parentChicken);
                break;
            case "Duck":
                Instantiate(duckPrefab, spawnPointDuck.position, spawnPointDuck.rotation,parentDuck);
                break;
            case "Goat1":
                Instantiate(goatPrefab1, spawnPointGoat1.position, spawnPointGoat1.rotation, parentGoat1);
                break;
            case "Goat2":
                Instantiate(goatPrefab2, spawnPointGoat2.position, spawnPointGoat2.rotation, parentGoat2);
                
                break;
            default:
                break;
        }
    }

    private void initializeAllAnimal()
    {
        int totCow = 0, totSheep = 0, totGoat1 = 0, totChicken = 0, totDuck = 0, totGoat2 = 0;
        totCow = PlayerSaveManager.Instance.GetItemCount("Cow");
        for (int i = 0; i < totCow; i++)
        {
            SpawnAnimal("Cow");
        }
        totSheep = PlayerSaveManager.Instance.GetItemCount("Sheep");
        for (int i = 0; i < totSheep; i++)
        {
            SpawnAnimal("Sheep");
        }
        totChicken = PlayerSaveManager.Instance.GetItemCount("Chicken");
        for (int i = 0; i < totChicken; i++)
        {
            SpawnAnimal("Chicken");
        }
        totDuck = PlayerSaveManager.Instance.GetItemCount("Duck");
        for (int i = 0; i < totDuck; i++)
        {
            SpawnAnimal("Duck");
        }
        totGoat1 = PlayerSaveManager.Instance.GetItemCount("Goat1");
        for (int i = 0; i < totGoat1; i++)
        {
            SpawnAnimal("Goat1");
        }
        totGoat2 = PlayerSaveManager.Instance.GetItemCount("Goat2");
        for (int i = 0; i < totGoat2; i++)
        {
            SpawnAnimal("Goat2");
        }
    }
}

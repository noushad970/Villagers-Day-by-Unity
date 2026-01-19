using UnityEngine;

public class FarmController : MonoBehaviour
{
    public LocalFarmSaveManager saveManager;
    public LandDataComponent[] allLands;
    public GameObject[] cropPrefabs; // Assign prefabs by name for planting

    private void Start()
    {
        LoadFarm();
    }

    // Save the whole farm
    public void SaveFarm()
    {
        FarmData farm = new FarmData();
        farm.lands = new LandData[allLands.Length];

        for (int i = 0; i < allLands.Length; i++)
        {
            farm.lands[i] = allLands[i].GetLandData();
        }

        saveManager.SaveFarm(farm);
    }

    // Load farm and instantiate crops
    public void LoadFarm()
    {
        FarmData farm = saveManager.LoadFarm();
        if (farm == null) return;

        for (int i = 0; i < farm.lands.Length; i++)
        {
            LandDataComponent landComp = allLands[i];
            LandData savedLand = farm.lands[i];

            landComp.isFertilized = savedLand.isFertilized;

            for (int j = 0; j < savedLand.cropAreas.Length; j++)
            {
                landComp.plantedCropNames[j] = savedLand.cropAreas[j].cropName;

                if (!string.IsNullOrEmpty(savedLand.cropAreas[j].cropName))
                {
                    // Instantiate crop prefab at local position
                    GameObject prefab = GetCropPrefab(savedLand.cropAreas[j].cropName);
                    if (prefab != null)
                    {
                        GameObject crop = Instantiate(prefab, landComp.cropAreasTransforms[j].position, Quaternion.identity);
                        crop.transform.SetParent(landComp.cropAreasTransforms[j]);
                    }
                }
            }
        }
        Debug.Log("✅ Farm loaded");
    }

    private GameObject GetCropPrefab(string cropName)
    {
        foreach (var prefab in cropPrefabs)
        {
            if (prefab.name == cropName)
                return prefab;
        }
        return null;
    }
}

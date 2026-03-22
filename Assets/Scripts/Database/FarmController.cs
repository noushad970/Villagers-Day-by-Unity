using UnityEngine;

public class FarmController : MonoBehaviour
{
    public LocalFarmSaveManager saveManager;
    public LandDataComponent[] allLands;
    public GameObject[] cropPrefabs;

    private void Start()
    {
        LoadFarm();
    }

    // ================= SAVE =================
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

    // ================= LOAD =================
    public void LoadFarm()
    {
        FarmData farm = saveManager.LoadFarm();
        if (farm == null) return;

        int landCount = Mathf.Min(allLands.Length, farm.lands.Length);

        for (int i = 0; i < landCount; i++)
        {
            LandDataComponent landComp = allLands[i];
            LandData savedLand = farm.lands[i];

            // Apply fertilization
            landComp.isFertilized = savedLand.isFertilized;
            landComp.landData.isFertilized = savedLand.isFertilized;

            // ✅ Update visuals (IMPORTANT)
            landComp.SendMessage("UpdateVisual", SendMessageOptions.DontRequireReceiver);

            int cropCount = Mathf.Min(
                landComp.cropAreasTransforms.Length,
                savedLand.cropAreas.Length
            );

            for (int j = 0; j < cropCount; j++)
            {
                Transform area = landComp.cropAreasTransforms[j];

                // 🔥 CLEAR OLD CROPS FIRST
                if (area.childCount > 0)
                {
                    for (int k = area.childCount - 1; k >= 0; k--)
                    {
                        Destroy(area.GetChild(k).gameObject);
                    }
                }

                string cropName = savedLand.cropAreas[j].cropName;
                landComp.plantedCropNames[j] = cropName;

                if (!string.IsNullOrEmpty(cropName))
                {
                    GameObject prefab = GetCropPrefab(cropName);

                    if (prefab != null)
                    {
                        GameObject crop = Instantiate(prefab, area.position, Quaternion.identity);
                        crop.transform.SetParent(area);
                    }
                    else
                    {
                        Debug.LogWarning("Prefab not found: " + cropName);
                    }
                }
            }
        }

        Debug.Log("✅ Farm loaded");
    }

    // ================= GET PREFAB =================
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
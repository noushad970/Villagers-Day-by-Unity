using UnityEngine;
using System;

[Serializable]
public class LandDataComponent : MonoBehaviour
{
    public bool isFertilized = false;           // Dart Land or Fertilized
    public Transform[] cropAreasTransforms;     // References to CropAreas
    public string[] plantedCropNames;           // Names of planted crops
    public LandData landData;
    public GameObject[] cropPrefabs;         // Assign prefabs by name for planting

    void Awake()
    {
        InitializeLandData();
        if (isFertilized)
        {
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
                       this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

    }

    // Initialize LandData
    public void InitializeLandData()
    {
        landData = new LandData();
        landData.landName = gameObject.name;
        landData.isFertilized = isFertilized;

        int cropCount = cropAreasTransforms.Length;
        landData.cropAreas = new CropAreaData[cropCount];
        plantedCropNames = new string[cropCount];

        for (int i = 0; i < cropCount; i++)
        {
            landData.cropAreas[i] = new CropAreaData
            {
                cropName = "",
                localPosition = cropAreasTransforms[i].localPosition,
                isPlanted = false
            };
            plantedCropNames[i] = "";
        }
    }

    // Fertilize this land
    public void FertilizeLand()
    {
        isFertilized = true;
        landData.isFertilized = true;
        Debug.Log($"{gameObject.name} fertilized!");
    }

    // Plant crop in CropArea
    public void PlantCrop(int index, string cropName)
    {
        if (!isFertilized)
        {
            Debug.LogWarning("Cannot plant crop: Land not fertilized");
            return;
        }

        if (index < 0 || index >= landData.cropAreas.Length)
        {
            Debug.LogWarning("Invalid CropArea index");
            return;
        }

        landData.cropAreas[index].cropName = cropName;
        landData.cropAreas[index].isPlanted = true;
        plantedCropNames[index] = cropName;

        Debug.Log($"Planted {cropName} at {gameObject.name} CropArea {index}");
    }

    // Get current LandData
    public LandData GetLandData()
    {
        for (int i = 0; i < landData.cropAreas.Length; i++)
        {
            landData.cropAreas[i].localPosition = cropAreasTransforms[i].localPosition;
            landData.cropAreas[i].isPlanted = !string.IsNullOrEmpty(plantedCropNames[i]);
            landData.cropAreas[i].cropName = plantedCropNames[i];
        }
        landData.isFertilized = isFertilized;
        return landData;
    }
    public void RemoveCrop(GameObject cropAreaObject)
    {
        if (cropAreaObject == null)
        {
            Debug.LogWarning("CropArea object is null");
            return;
        }

        for (int i = 0; i < cropAreasTransforms.Length; i++)
        {
            if (cropAreasTransforms[i].gameObject == cropAreaObject.transform.parent.gameObject)
            {
                if (!landData.cropAreas[i].isPlanted)
                {
                    Debug.Log("No crop planted on this CropArea");
                    return;
                }

                // Clear saved data
                landData.cropAreas[i].cropName = "";
                landData.cropAreas[i].isPlanted = false;
                plantedCropNames[i] = "";

                // Remove visual crop GameObject
                if (cropAreasTransforms[i].childCount > 0)
                {
                    Destroy(cropAreasTransforms[i].GetChild(0).gameObject);
                }

                Debug.Log($"Crop removed from {gameObject.name} → {cropAreaObject.name}");
                return;
            }
        }

        Debug.LogWarning("CropArea GameObject not found in this LandDataComponent");
    }

}

using System.Collections;
using UnityEngine;

public class CropGrowUp : MonoBehaviour
{
    [SerializeField] private float cropGrowthTime; // Time in seconds for the crop to fully grow
    [SerializeField] private float cropWastedTime;
    [SerializeField] private GameObject[] growthStages; // Array of GameObjects representing each growth stage
    [SerializeField] private GameObject wastedStage;
    [SerializeField] private string cropName;
    [SerializeField] private float minSizeX, minSizeY, minSizeZ;
    [SerializeField] private float maxSizeX, maxSizeY, maxSizeZ;
    [SerializeField] private float totTime = 2f;

    private void Awake()
    {
        StartCoroutine(cropGrowing());
        StartScaling();

    }

    
    public void StartScaling()
    {
        foreach(GameObject target1 in growthStages)
        {

            StartCoroutine(ScaleRoutine(target1));
        }
       
    }

    IEnumerator ScaleRoutine(GameObject target)
    {
        if (target == null || totTime <= 0f)
            yield break;

        Vector3 minSize = new Vector3(minSizeX, minSizeY, minSizeZ);
        Vector3 maxSize = new Vector3(maxSizeX, maxSizeY, maxSizeZ);

        float elapsed = 0f;
        target.transform.localScale = minSize;

        while (elapsed < totTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / totTime;
            target.transform.localScale = Vector3.Lerp(minSize, maxSize, t);
            yield return null;
        }

        target.transform.localScale = maxSize;
    }
    IEnumerator cropGrowing()
    {
        for (int i = 0; i < growthStages.Length; i++)
        {
            growthStages[i].SetActive(false);
        }
        float stageDuration = cropGrowthTime / growthStages.Length;
        growthStages[0].SetActive(true);
        yield return new WaitForSeconds(stageDuration);
        growthStages[0].SetActive(false);
        growthStages[1].SetActive(true);
        yield return new WaitForSeconds(stageDuration);
        growthStages[1].SetActive(false);
        growthStages[2].SetActive(true);
        if(growthStages.Length == 4)
        {
            yield return new WaitForSeconds(stageDuration);
            growthStages[2].SetActive(false);
            growthStages[3].SetActive(true);

            if(wastedStage!= null)
            {
                yield return new WaitForSeconds(cropWastedTime);
                wastedStage.SetActive(true);
                growthStages[3].SetActive(false);
            }
        }
        else {             
            if(wastedStage!= null)
            {
                yield return new WaitForSeconds(cropWastedTime);
                wastedStage.SetActive(true);
                growthStages[2].SetActive(false);
            }
        }

    }
    public bool isCropGrown()
    {
        if(growthStages[growthStages.Length - 1].activeSelf)
        {
            return true;
        }
        return false;
    }
}

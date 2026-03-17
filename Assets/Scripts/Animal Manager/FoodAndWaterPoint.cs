using System.Collections;
using UnityEngine;

public class FoodAndWaterPoint : MonoBehaviour
{
    public GameObject foodObject;
    public bool checkFood()
    {
        if (foodObject.activeSelf)
        {
            return true;
        }
        return false;
    }
    private void Update()
    {
        if (!checkFood()) {
            StartCoroutine(refillFood());
        }
    }
     IEnumerator refillFood()
    {
        yield return new WaitForSeconds(5f);
        foodObject.SetActive(true);
    }
}

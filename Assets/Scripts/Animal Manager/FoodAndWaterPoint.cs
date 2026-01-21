using UnityEngine;

public class FoodAndWaterPoint : MonoBehaviour
{
    public GameObject foodObject;
    public bool checkFood()
    {
        if (foodObject.activeSelf)
        {
            Debug.Log("Food is available.");
            return true;
        }
        Debug.Log("Food is not available.");
        return false;
    }
}

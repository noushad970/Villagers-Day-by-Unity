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

    public void refillFood()
    {
        foodObject.SetActive(true);
    }
}

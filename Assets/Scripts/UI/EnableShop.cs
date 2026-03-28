using UnityEngine;

public class EnableShop : MonoBehaviour
{
    public GameObject shopSection;
    public GameObject colliders;
    private void Start()
    {
        colliders.SetActive(false);
    }
    public void enableShopSection()
    {
        shopSection.SetActive(true);
    }

    private void Update()
    {
        if (NPCShopman.isShopOpen)
        {
            colliders.SetActive(true);
        }
        else
        {
            colliders.SetActive(false);
        }
    }
}

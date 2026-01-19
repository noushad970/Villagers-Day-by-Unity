using UnityEngine;

public class PlayerCollectionState : MonoBehaviour
{
    [SerializeField] private GameObject[] handPickedItems;
    public void AddingCropSeedToHand(string seedName)
    {
        disableAllCropsAndSeed();
        for (int i = 0; i < handPickedItems.Length; i++)
        {
            if (seedName == handPickedItems[i].name)
            {
                handPickedItems[i].SetActive(true);
            }
        }
    }
    public void AddingCropToHand(string cropName)
    {

        disableAllCropsAndSeed();
        for (int i = 0; i < handPickedItems.Length; i++)
        {
            if (cropName == handPickedItems[i].name)
            {
                handPickedItems[i].SetActive(true);
            }
        }

    }

    private void disableAllCropsAndSeed()
    {
        ActivateCraftingTool.Instance.setActiveAllToolsFalse();
        for (int i = 0; i < handPickedItems.Length; i++) {
            handPickedItems[i].SetActive(false);
        }

    }
}

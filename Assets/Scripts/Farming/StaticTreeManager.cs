using UnityEngine;

public class StaticTreeManager : MonoBehaviour
{
    public GameObject[] trees; // Assign all tree objects manually in Inspector

    private string GetKey(int index)
    {
        return "TreeDestroyed_" + index;
    }

    void Start()
    {
        LoadTreeStates();
    }

    void LoadTreeStates()
    {
        for (int i = 0; i < trees.Length; i++)
        {
            if (PlayerPrefs.GetInt(GetKey(i), 0) == 1)
            {
                trees[i].SetActive(false); // Hide destroyed tree
            }
        }
    }

    public void DestroyTree(int index)
    {
        if (index < 0 || index >= trees.Length) return;

        trees[index].SetActive(false);
        PlayerPrefs.SetInt(GetKey(index), 1);
        PlayerPrefs.Save();
    }
}
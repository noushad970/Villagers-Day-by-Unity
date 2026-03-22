using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeData
{
    public string treeID;
    public string plantName;
    public Vector3 position;
    public string state;
}

[System.Serializable]
public class TreeDataList
{
    public List<TreeData> trees = new List<TreeData>();
}

public class TreeSaveManager : MonoBehaviour
{
    private const string TREE_KEY = "TREE_DATA";

    public GameObject[] treePrefab;
    public List<GameObject> plantedTrees = new List<GameObject>();

    private TreeDataList treeDataList = new TreeDataList();

    void Start()
    {
        LoadTrees();
    }

    // ================= SAVE NEW TREE =================
    public void SaveTree(GameObject tree, string state = "Planted")
    {
        TreeID id = tree.GetComponent<TreeID>();

        if (id == null)
        {
            Debug.LogError("TreeID component missing!");
            return;
        }

        // Generate ID if empty
        if (string.IsNullOrEmpty(id.treeID))
        {
            id.treeID = System.Guid.NewGuid().ToString();
        }

        // Prevent duplicate
        if (treeDataList.trees.Exists(t => t.treeID == id.treeID))
        {
            Debug.Log("Tree already saved: " + id.treeID);
            return;
        }

        TreeData data = new TreeData
        {
            treeID = id.treeID,
            plantName = tree.name.Replace("(Clone)", ""),
            position = tree.transform.position,
            state = state
        };

        treeDataList.trees.Add(data);
        plantedTrees.Add(tree);

        SaveToPrefs();

        Debug.Log("Tree saved successfully: " + id.treeID);
    }

    // ================= CHANGE STATE =================
    public void ChangeTreeState(GameObject tree, string newState)
    {
        TreeID id = tree.GetComponent<TreeID>();

        if (id == null) return;

        TreeData found = treeDataList.trees.Find(t => t.treeID == id.treeID);

        if (found != null)
        {
            found.state = newState;
            SaveToPrefs();
        }
        else
        {
            Debug.LogWarning("Tree data not found!");
        }
    }

    // ================= REMOVE TREE =================
    public void RemoveTree(GameObject tree)
    {
        ChangeTreeState(tree, "Cutted");

        if (plantedTrees.Contains(tree))
        {
            plantedTrees.Remove(tree);
        }
    }

    // ================= SAVE =================
    void SaveToPrefs()
    {
        string json = JsonUtility.ToJson(treeDataList, false); // no pretty print (smaller size)

        PlayerPrefs.SetString(TREE_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Tree data saved (PlayerPrefs)");
    }

    // ================= LOAD =================
    public void LoadTrees()
    {
        if (!PlayerPrefs.HasKey(TREE_KEY))
        {
            Debug.Log("No tree save found.");
            treeDataList = new TreeDataList();
            return;
        }

        string json = PlayerPrefs.GetString(TREE_KEY);

        treeDataList = JsonUtility.FromJson<TreeDataList>(json);

        if (treeDataList == null || treeDataList.trees == null)
        {
            treeDataList = new TreeDataList();
            return;
        }

        // 🔥 Prevent duplicate spawn on reload
        foreach (var tree in plantedTrees)
        {
            if (tree == null) continue;

            // ✅ Only destroy scene objects, NOT prefabs
            if (tree.scene.IsValid())
            {
                Destroy(tree);
            }
        }
        plantedTrees.Clear();

        foreach (TreeData data in treeDataList.trees)
        {
            if (data.state == "Cutted")
                continue;

            foreach (var prefab in treePrefab)
            {
                if (prefab.name == data.plantName)
                {
                    GameObject tree = Instantiate(prefab, data.position, Quaternion.identity);

                    TreeID id = tree.GetComponent<TreeID>();
                    if (id != null)
                        id.treeID = data.treeID;

                    plantedTrees.Add(tree);

                    break;
                }
            }
        }

        Debug.Log("Tree data loaded (PlayerPrefs)");
    }
}
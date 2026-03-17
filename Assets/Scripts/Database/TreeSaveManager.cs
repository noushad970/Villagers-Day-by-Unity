using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    public string saveFileName = "TreeData.json";

    public GameObject[] treePrefab;

    public List<GameObject> plantedTrees = new List<GameObject>();

    private TreeDataList treeDataList = new TreeDataList();

    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    void Start()
    {
        LoadTrees();
    }
    private void Update()
    {

    }

    // SAVE NEW TREE
    public void SaveTree(GameObject tree, string state = "Planted")
    {
        TreeID id = tree.GetComponent<TreeID>();

        // Generate ID if empty
        if (id == null)
        {
            Debug.LogError("TreeID component missing!");
            return;
        }

        if (string.IsNullOrEmpty(id.treeID))
        {
            id.treeID = System.Guid.NewGuid().ToString();
        }

        // prevent duplicate save
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

        WriteToFile();

        Debug.Log("Tree saved successfully: " + id.treeID);
    }

    // CHANGE TREE STATE
    public void ChangeTreeState(GameObject tree, string newState)
    {
        TreeID id = tree.GetComponent<TreeID>();

        TreeData found = treeDataList.trees.Find(t => t.treeID == id.treeID);

        Debug.Log(found != null ?
            $"Found tree data: {found.plantName} at {found.position} with state {found.state}"
            : "Tree data not found!");

        if (found != null)
        {
            found.state = newState;

            WriteToFile();
        }
    }

    // REMOVE TREE (CUT TREE)
    public void RemoveTree(GameObject tree)
    {
        ChangeTreeState(tree, "Cutted");

        if (plantedTrees.Contains(tree))
        {
            plantedTrees.Remove(tree);
        }
    }

    // WRITE JSON
    public void WriteToFile()
    {
        string json = JsonUtility.ToJson(treeDataList, true);

        File.WriteAllText(SavePath, json);

        Debug.Log("Tree data saved: " + SavePath);
    }

    // LOAD TREES
    public void LoadTrees()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        string json = File.ReadAllText(SavePath);

        treeDataList = JsonUtility.FromJson<TreeDataList>(json);

        if (treeDataList == null || treeDataList.trees == null)
        {
            treeDataList = new TreeDataList();
            return;
        }

        foreach (TreeData data in treeDataList.trees)
        {
            if (data.state == "Cutted")
                continue;

            for (int i = 0; i < treePrefab.Length; i++)
            {
                if (treePrefab[i].name == data.plantName)
                {
                    GameObject tree = Instantiate(treePrefab[i], data.position, Quaternion.identity);

                    TreeID id = tree.GetComponent<TreeID>();

                    if (id != null)
                        id.treeID = data.treeID;

                    plantedTrees.Add(tree);

                    Debug.Log($"Loaded tree: {data.plantName} at {data.position}");

                    break;
                }
            }
        }
    }
}
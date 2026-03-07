using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;

[System.Serializable]
public class TreeData
{
    public string plantName;
    public Vector3 position;
    public string state; // "Planted", "Grown", "Cutted"
}

[System.Serializable]
public class TreeDataList
{
    public List<TreeData> trees = new List<TreeData>();
}

public class TreeSaveManager : MonoBehaviour
{
    public string saveFileName = "TreeData.json";
    public List<GameObject> plantedTrees = new List<GameObject>();
    public GameObject[] treePrefab;
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    private TreeDataList treeDataList = new TreeDataList();

    void Start()
    {
        LoadTrees();
        
    }
    //private void Update()
    //{
    //    for (int i = 0; i < treePrefab.Length; i++)
    //    {
            
    //        if(treePrefab[i] == null) continue;
    //        {

    //            ChangeTreeState(treePrefab[i], "Cutted");
    //            Debug.Log("Available Tree Prefab: " + treePrefab[i].name);
    //            plantedTrees.Remove(treePrefab[i]);
    //            GetTreeState(treePrefab[i]);

    //        }
    //    }
    //}

    // Save a new tree
    public void SaveTree(GameObject tree, string state = "Planted")
    {
        TreeData data = new TreeData
        {
            plantName = tree.name.Replace("(Clone)", ""),
            position = tree.transform.position,
            state = state
        };

        treeDataList.trees.Add(data);
        plantedTrees.Add(tree);
        WriteToFile();
    }

    // Change the state of a tree
    public void ChangeTreeState(GameObject tree, string newState)
    {
        string treeName = tree.name.Replace("(Clone)", "");
        Vector3 pos = tree.transform.position;

        // Find the tree in saved data
        TreeData found = treeDataList.trees.Find(t => t.plantName == treeName && t.position == pos);
        if (found != null)
        {
            found.state = newState;
            WriteToFile();
            Debug.Log($"Tree {treeName} at {pos} state changed to {newState}");
        }
        else
        {
            Debug.LogWarning("Tree not found in saved data!");
        }
    }

    // Get tree state
    public string GetTreeState(GameObject tree)
    {
        string treeName = tree.name.Replace("(Clone)", "");
        Vector3 pos = tree.transform.position;

        TreeData found = treeDataList.trees.Find(t => t.plantName == treeName && t.position == pos);
        return found != null ? found.state : "Unknown";
    }

    // Write current data to JSON
    public void WriteToFile()
    {
        string json = JsonUtility.ToJson(treeDataList, true);
        File.WriteAllText(SavePath, json);
    }

    // Load trees
    public void LoadTrees()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        treeDataList = JsonUtility.FromJson<TreeDataList>(json);

        foreach (TreeData data in treeDataList.trees)
        {
            // Optional: spawn the tree prefab according to state
            // If state == "Cutted", you could disable the tree GameObject in scene
            for(int i=0;i<treePrefab.Length;i++)
            {
                if(treePrefab[i].name == data.plantName)
                {
                    Debug.Log($"Loading tree: {data.plantName} at {data.position} with state {data.state}");
                    if (data.state == "Cutted")
                    {

                    }
                    else
                    {
                        GameObject tree = Instantiate(treePrefab[i], data.position, Quaternion.identity);
                        plantedTrees.Add(tree);
                    }
                    break;
                }
            }
        }
    }

}

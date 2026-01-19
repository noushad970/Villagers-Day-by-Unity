using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteAlways]
public class MudChildrenDuplicator : MonoBehaviour
{
    [Header("Assign Mud Objects and Children")]
    public GameObject[] mudObjects;   // All Mud_01 objects
    public GameObject[] childrenToCopy; // All children to duplicate

    [ContextMenu("Duplicate Children To All MudObjects")]
    public void DuplicateChildren()
    {
        if (mudObjects == null || childrenToCopy == null)
        {
            Debug.LogError("MudObjects or ChildrenToCopy not assigned!");
            return;
        }

        foreach (GameObject mud in mudObjects)
        {
            foreach (GameObject child in childrenToCopy)
            {
                if (child == null || mud == null) continue;

                // Duplicate the child
                GameObject copy = Instantiate(child, mud.transform);

                // Optional: reset local position/rotation/scale
                copy.transform.localPosition = child.transform.localPosition;
                copy.transform.localRotation = child.transform.localRotation;
                copy.transform.localScale = child.transform.localScale;

                copy.name = child.name; // keep same name
            }
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif

        Debug.Log($"Duplicated {childrenToCopy.Length} children into {mudObjects.Length} MudObjects.");
    }
}

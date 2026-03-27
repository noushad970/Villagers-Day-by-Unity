using UnityEngine;
using TMPro;

public class OxygenSystem : MonoBehaviour
{
    [Header("Tree Settings")]
    public string treeLayerName = "Tree";
    public int normalTreeCount = 50; // 50 trees = 100%
    public int minimumTreeCount = 20; // 40% threshold

    [Header("UI")]
    public TextMeshProUGUI oxygenText;

    [Header("Colors")]
    public Color normalColor = Color.green;
    public Color lowColor = Color.red;

    int treeLayer;

    void Start()
    {
        treeLayer = LayerMask.NameToLayer(treeLayerName);
    }

    void Update()
    {
        UpdateOxygen();
    }

    void UpdateOxygen()
    {
        int treeCount = CountTrees();

        // Calculate percentage
        float percentage = ((float)treeCount / normalTreeCount) * 100f;
        percentage = Mathf.Clamp(percentage, 0f, 100f);

        // Determine condition
        string condition;

        if (percentage < 40f)
        {
            condition = "LOW Plant more trees!";
            oxygenText.color = lowColor;
        }
        else
        {
            condition = "NORMAL";
            oxygenText.color = normalColor;
        }

        // Update UI text
        oxygenText.text = $"Oxygen: {percentage:0}%\nStatus: {condition}";
    }

    int CountTrees()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == treeLayer)
                count++;
        }

        return count;
    }
}
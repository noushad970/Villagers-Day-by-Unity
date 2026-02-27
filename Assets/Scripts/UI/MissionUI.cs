using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private MissionPanel[] missionPanels; // Size = 4
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject MissionPanel;
    private void Start()
    {
        // Example missions
        StartCoroutine(wait());
        backButton.onClick.AddListener(clickOnBack);


    }
    IEnumerator wait()
    {
               yield return new WaitForSeconds(3f);

        missionPanels[0].LoadMission(PlayerSaveManager.Instance.GetItemCount("CurrentMission1Index"));
        missionPanels[1].LoadMission(PlayerSaveManager.Instance.GetItemCount("CurrentMission2Index"));
        missionPanels[2].LoadMission(PlayerSaveManager.Instance.GetItemCount("CurrentMission3Index"));
        missionPanels[3].LoadMission(PlayerSaveManager.Instance.GetItemCount("CurrentMission4Index"));

    }
    private void clickOnBack()
    {
                MissionPanel.SetActive(false);

    }
    //[Header("UI References (Drag & Drop in Inspector)")]
    //[SerializeField] private TMP_Text titleText;
    //[SerializeField] private Transform itemListParent;
    //[SerializeField] private GameObject itemRowPrefab;
    //[SerializeField] private Button loadButton;
    //[SerializeField] private TMP_InputField missionInput;

    //private void Start()
    //{
    //    if (loadButton != null)
    //        loadButton.onClick.AddListener(LoadCurrentMission);

    //    // Example auto-load
    //    LoadMission(50);
    //}

    ///// <summary>
    ///// Load and display mission requirements
    ///// </summary>
    //public void LoadMission(int missionIndex)
    //{
    //    var requirements = MissionData.GetMissionRequirements(missionIndex);

    //    if (requirements == null || requirements.Count == 0)
    //    {
    //        Debug.LogError($"Failed to load mission {missionIndex}");
    //        return;
    //    }

    //    // Update title
    //    if (titleText != null)
    //        titleText.text = $"Mission {missionIndex} Requirements ({requirements.Count} items)";

    //    // Clear old rows safely
    //    ClearItemRows();

    //    // Sort alphabetically
    //    var sortedReq = requirements.OrderBy(kvp => kvp.Key).ToList();

    //    // Create rows
    //    foreach (var kvp in sortedReq)
    //    {
    //        CreateItemRow(kvp.Key, kvp.Value);
    //    }

    //    // Force layout rebuild (important for UI refresh)
    //    LayoutRebuilder.ForceRebuildLayoutImmediate(itemListParent.GetComponent<RectTransform>());
    //}

    ///// <summary>
    ///// Load mission from input field
    ///// </summary>
    //public void LoadCurrentMission()
    //{
    //    if (missionInput != null && int.TryParse(missionInput.text, out int index))
    //    {
    //        LoadMission(index);
    //    }
    //}

    //private void ClearItemRows()
    //{
    //    for (int i = itemListParent.childCount - 1; i >= 0; i--)
    //    {
    //        Destroy(itemListParent.GetChild(i).gameObject);
    //    }
    //}

    //private void CreateItemRow(string itemName, int quantity)
    //{
    //    if (itemRowPrefab == null)
    //    {
    //        Debug.LogError("ItemRowPrefab is not assigned!");
    //        return;
    //    }

    //    GameObject row = Instantiate(itemRowPrefab, itemListParent);

    //    // Find text components safely
    //    TMP_Text nameText = row.transform.Find("ItemNameText")?.GetComponent<TMP_Text>();
    //    TMP_Text qtyText = row.transform.Find("QuantityText")?.GetComponent<TMP_Text>();

    //    if (nameText == null || qtyText == null)
    //    {
    //        Debug.LogError("Prefab must contain TMP_Text children named 'ItemNameText' and 'QuantityText'");
    //        return;
    //    }

    //    nameText.text = itemName + ":";
    //    qtyText.text = quantity.ToString();
    //}
}
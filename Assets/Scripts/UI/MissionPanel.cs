using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform itemListParent;
    [SerializeField] private GameObject itemRowPrefab;
    [SerializeField] private bool canDelivered = true;
    [SerializeField] private AIAirplane airplane;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button DeliverButton;
    private int foodItemCount = 0;
    private string foodname;
    private void Start()
    {
        DeliverButton.onClick.AddListener(DeliverFood);
    }
    public void LoadMission(int missionIndex)
    {
        var requirements = MissionData.GetMissionRequirements(missionIndex);

        if (requirements == null || requirements.Count == 0)
        {
            Debug.LogError($"Mission {missionIndex} not found");
            return;
        }


        ClearItems();

        var sorted = requirements.OrderBy(x => x.Key).ToList();

        foreach (var item in sorted)
        {
            GameObject row = Instantiate(itemRowPrefab, itemListParent);

            TMP_Text nameText = row.transform.Find("ItemNameText")?.GetComponent<TMP_Text>();
            TMP_Text qtyText = row.transform.Find("QuantityText")?.GetComponent<TMP_Text>();

            if (nameText != null) nameText.text = item.Key + ":";
            if (qtyText != null) qtyText.text = item.Value.ToString()+"/"+PlayerSaveManager.Instance.GetItemCount(item.Key);
            if (item.Value > PlayerSaveManager.Instance.GetItemCount(item.Key))
            {
                canDelivered = false;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(itemListParent.GetComponent<RectTransform>());
    }
    public void DeliverFood()
    {
        if (airplane.getFlyingStatus())
        {
            NoticeUI.Instance.ShowNotice("Airplane is still flying. Cannot deliver food.");
            return;
        }
        if(canDelivered)
        {
            airplane.StartFly();
            if(title.text=="Mission 1")
            {
                var requirements = MissionData.GetMissionRequirements(PlayerSaveManager.Instance.GetItemCount("CurrentMission1Index"));
                var sorted = requirements.OrderBy(x => x.Key).ToList();

                foreach (var item in sorted)
                {
                    PlayerSaveManager.Instance.AddItem(item.Key, -item.Value);
                }
            }
            else if (title.text == "Mission 2")
            {
                var requirements = MissionData.GetMissionRequirements(PlayerSaveManager.Instance.GetItemCount("CurrentMission2Index")); 
                var sorted = requirements.OrderBy(x => x.Key).ToList();

                foreach (var item in sorted)
                {
                    PlayerSaveManager.Instance.AddItem(item.Key, -item.Value);
                }
            }
            else if (title.text == "Mission 3")
            {
                var requirements = MissionData.GetMissionRequirements(PlayerSaveManager.Instance.GetItemCount("CurrentMission3Index"));
                var sorted = requirements.OrderBy(x => x.Key).ToList();

                foreach (var item in sorted)
                {
                    PlayerSaveManager.Instance.AddItem(item.Key, -item.Value);
                }
            }
            else if (title.text == "Mission 4")
            {
                var requirements = MissionData.GetMissionRequirements(PlayerSaveManager.Instance.GetItemCount("CurrentMission4Index"));
                var sorted = requirements.OrderBy(x => x.Key).ToList();

                foreach (var item in sorted)
                {
                    PlayerSaveManager.Instance.AddItem(item.Key, -item.Value);
                }
            }

        }
        else
        {
            NoticeUI.Instance.ShowNotice("Cannot deliver food. Requirements not met.");
        }
    }
    private void ClearItems()
    {
        for (int i = itemListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(itemListParent.GetChild(i).gameObject);
        }
    }
}
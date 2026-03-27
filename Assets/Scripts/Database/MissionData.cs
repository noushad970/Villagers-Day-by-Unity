using System;
using System.Collections.Generic;
using UnityEngine;

public static class MissionData
{
    private static readonly string[] missions = {
        "1 Egg, 1 Milk, 1 Wheat, 1 Bean",
        "14 Bean, 18 Sardine, 7 Salmon, 8 Tuna",
        "11 Chilli, 11 Pumkin, 14 Mackerel, 5 Bean, 7 Wool, 9 Meat",
        "12 Broccoli, 12 Rohu, 9 Cabbage",
        "13 Tilapia, 3 Tuna, 4 Milk",
        "10 Sardine, 2 Wood, 3 Tomato, 3 Wheat, 4 Corn, 8 Cod, 8 Mackerel",
        "12 Pumkin, 12 Tilapia, 3 Wheat, 6 Chilli, 7 Cod, 9 Wood",
        "11 Chilli, 11 Cod, 18 Milk, 2 Pumkin, 2 Tilapia, 8 Pepper, 8 Rohu",
        "13 Corn, 15 Mackerel, 16 Broccoli, 5 Egg, 7 Pumkin, 9 Watermelon",
        "13 Pumkin, 14 Milk, 19 Pepper, 19 Tuna",
        "14 Broccoli, 2 Pepper, 4 Carrot, 5 Salmon, 6 Catfish",
        "1 Rohu, 11 Salmon, 18 Broccoli, 18 Sardine, 4 Pumkin, 4 kg Rice, 9 Tilapia",
        "17 Bean, 17 Chilli, 4 Milk, 6 Tilapia, 9 Hilsa",
        "1 Carrot, 1 Salmon, 11 Sardine, 16 Corn, 17 Cod, 18 Wood, 4 Wheat, 6 Tomato",
        "19 Meat, 2 kg Rice, 3 Pepper, 3 Tomato, 8 Wood",
        "14 kg Rice, 16 Catfish, 17 Tuna, 18 Milk, 18 Wood, 6 Broccoli, 7 Carrot, 9 Wool",
        "11 Wheat, 15 Egg, 15 kg Suger, 17 Corn, 3 Cod, 4 Tomato, 8 Carp, 8 Rohu",
        "1 Pepper, 19 Mackerel, 8 Tuna",
        "2 Beetroot, 3 Cod, 8 Egg",
        "16 Broccoli, 18 Pepper, 19 Carp, 5 Pumkin, 7 Salmon",
        "12 kg Suger, 14 Hilsa, 14 Meat, 14 kg Flour, 15 Corn, 4 Catfish, 4 Pepper",
        "14 Watermelon, 15 kg Suger, 18 Cabbage, 4 Beetroot, 5 Rohu, 7 Cod, 7 kg Flour, 8 Carp",
        "15 Tilapia, 18 Pepper, 3 Pumkin, 4 kg Flour",
        "1 Cod, 3 Tuna, 8 Wood",
        "13 Hilsa, 13 kg Flour, 2 Catfish, 6 Corn",
        "10 Pumkin, 14 kg Suger, 15 Rohu",
        "10 Milk, 11 Carrot, 18 Egg, 19 Carp, 2 Catfish, 2 Corn, 2 Tuna, 7 Meat",
        "17 Mackerel, 2 Salmon, 6 Catfish",
        "13 kg Flour, 19 Wood, 19 kg Suger, 2 Carp, 4 Chilli, 8 Broccoli, 8 Sardine",
        "11 Carp, 13 Salmon, 5 Watermelon, 7 Hilsa, 8 Mackerel, 9 Broccoli, 9 Wood",
        "17 Broccoli, 18 Watermelon, 19 Cod, 3 Tilapia, 4 Tomato, 5 kg Flour, 7 Wool, 9 Bean",
        "10 Wheat, 15 Broccoli, 17 Tomato, 18 Pepper, 6 kg Rice",
        "10 Carp, 4 Wood, 5 Tuna",
        "10 kg Rice, 11 Tuna, 7 Carrot, 7 Milk, 9 Cabbage",
        "1 Salmon, 11 Catfish, 14 Cod, 2 Pumkin, 5 kg Rice, 6 Broccoli, 9 Beetroot, 9 kg Flour",
        "12 Hilsa, 14 Broccoli, 18 Bean, 18 Tuna, 19 kg Rice, 2 Egg, 5 Cabbage, 5 Tilapia",
        "12 Tomato, 2 Beetroot, 7 Wheat, 8 kg Rice",
        "1 Hilsa, 11 Milk, 14 Carrot, 14 Tuna, 6 Sardine, 6 Wheat, 6 Wool, 8 Cabbage",
        "10 Cabbage, 12 Egg, 13 kg Flour, 15 Chilli, 16 Meat, 2 Milk, 7 Pumkin, 8 Pepper",
        "11 Bean, 13 Pepper, 3 Corn, 9 Carp",
        "1 Salmon, 11 Cod, 18 Wheat, 4 Rohu, 9 Carp",
        "11 Beetroot, 12 Pumkin, 14 Cabbage, 14 Mackerel",
        "1 Cabbage, 12 Pumkin, 14 Beetroot, 14 Salmon, 17 Rohu, 18 Mackerel, 7 Corn",
        "10 Watermelon, 11 Carp, 4 kg Suger",
        "13 Rohu, 14 Watermelon, 18 Tomato, 19 kg Suger, 5 Carp, 6 Egg, 7 Hilsa",
        "10 Rohu, 11 Tomato, 14 Wood, 19 Bean, 7 Tuna",
        "10 Carp, 11 Salmon, 17 Corn, 3 Catfish, 3 kg Suger, 6 Tilapia",
        "1 Pepper, 2 Meat, 5 Tomato, 7 Carp",
        "14 kg Flour, 15 Sardine, 19 Wool, 3 Catfish",
        "1 Rohu, 14 Pepper, 4 Catfish, 5 Egg"
    };

    /// <summary>
    /// Returns a dictionary of required items for the given mission index (1-50).
    /// Key: Item name (e.g., "Bean", "Rice", "Suger" - "kg " prefix removed).
    /// Value: Exact quantity needed.
    /// </summary>
    public static Dictionary<string, int> GetMissionRequirements(int missionIndex)
    {
        if (missionIndex < 1 || missionIndex > 50)
        {
            Debug.LogError("Invalid mission index: " + missionIndex + ". Must be 1-50.");
            return new Dictionary<string, int>();
        }

        string line = missions[missionIndex - 1];
        var requirements = new Dictionary<string, int>();

        // Split by ", " to get individual "qty item" parts
        string[] parts = line.Split(new string[] { ", " }, StringSplitOptions.None);

        foreach (string part in parts)
        {
            if (string.IsNullOrWhiteSpace(part)) continue;

            // Split by spaces
            string[] words = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 2) continue;

            // Parse quantity (first word)
            if (!int.TryParse(words[0], out int qty)) continue;

            // Join the rest as item name
            string item = string.Join(" ", words, 1, words.Length - 1);

            // Remove "kg " prefix if present (for Flour, Rice, Suger)
            if (item.StartsWith("kg ", StringComparison.OrdinalIgnoreCase))
            {
                item = item.Substring(3).Trim();
            }

            // Add or overwrite (no duplicates expected)
            requirements[item] = qty;
        }

        return requirements;
    }

    /// <summary>
    /// Example usage: Logs all items needed for mission 50.
    /// Call this in Update() or a button for testing.
    /// </summary>
    public static void LogMissionRequirements(int missionIndex)
    {
        var req = GetMissionRequirements(missionIndex);
        foreach (var kvp in req)
        {
            Debug.Log($"- {kvp.Key}: {kvp.Value}");
        }
    }
}
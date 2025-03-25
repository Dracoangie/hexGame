using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HexTypeEntry
{
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    [TextArea] public string description;
    public Color color;
}


[System.Serializable]
public class HexTypeCategory
{
    public string categoryName;
    public List<HexTypeEntry> entries = new List<HexTypeEntry>();
}

public class HexTypeManager : MonoBehaviour
{
    public HexContentDatabase database;

    public HexTypeEntry GetEntry(string category, string id)
    {
        foreach (var cat in database.categories)
        {
            if (cat.categoryName == category)
                return cat.entries.Find(e => e.id == id);
        }
        return null;
    }
}

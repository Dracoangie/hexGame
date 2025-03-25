using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Hex/Content Database")]
public class HexContentDatabase : ScriptableObject
{
    public List<HexTypeCategory> categories = new List<HexTypeCategory>();
}

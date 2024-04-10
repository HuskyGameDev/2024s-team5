using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Loot Item")]
public class LootItem : ScriptableObject
{
    public GameObject prefab;
    public string lootName;
    public Sprite lootIcon;
    [TextArea(3, 5)]
    public string description;
}

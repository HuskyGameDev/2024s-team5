using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<LootItem> items = new List<LootItem>();

    public void AddItem(LootItem item)
    {
        items.Add(item);
    }

    public void RemoveItem(LootItem item)
    {
        items.Remove(item);
    }
}

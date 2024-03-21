using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    
    private void AddToInventory(LootItem _item, int _amount)
    {
        inventory.AddItem(_item, _amount);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<Item>();
        if(item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(collision.gameObject);
        }
    }
}

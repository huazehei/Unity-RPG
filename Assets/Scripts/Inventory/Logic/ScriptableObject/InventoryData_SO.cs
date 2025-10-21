using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new();

    public void AddItem(ItemData_SO newItemData, int newAmounts)
    {
        if (newItemData.stackable)
        {
            bool found = false;
            foreach (var item in items)
            {
                if (item.itemData == newItemData)
                {
                    item.amounts += newAmounts;
                    found = true;
                    break;
                }
            }
        
            if (found)
                return;
        }
        

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData == null)
            {
                items[i].itemData = newItemData;
                items[i].amounts = newAmounts;
                return;
            }
        }
    }
}
[Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;
    public int amounts;
}

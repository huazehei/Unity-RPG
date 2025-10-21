using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUI : MonoBehaviour
{
    public Image icon = null; 
    public Text amount = null;
    public InventoryData_SO dataSO { get; set; }
    public int Index { get; set; } = -1;
    //只是用于任务系统获取奖励物品数据的成员变量
    public ItemData_SO itemData;

    public void SetupItemUI(ItemData_SO item, int itemAmount)
    {
        if (itemAmount == 0)
        {
            dataSO.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }

        if (itemAmount < 0)
            item = null;
        if (item != null)
        {
            itemData = item; 
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    public InventoryItem GetItem()
    { 
        Debug.Log(Index);
        return dataSO.items[Index];
    }
}

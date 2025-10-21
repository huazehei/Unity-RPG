using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType{BAG,ACTION,WEAPON,ARMOR}
public class SlotUI : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;
  
    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.dataSO = InventoryManager.Instance.bagData;
                break;
            case SlotType.ARMOR:
                itemUI.dataSO = InventoryManager.Instance.statsData;
                if(itemUI.dataSO.items[itemUI.Index].itemData != null)
                    GameManager.Instance.playerStates.ChangeArmour(itemUI.dataSO.items[itemUI.Index].itemData);
                else
                    GameManager.Instance.playerStates.UnEquipArmour();
                break;
            case SlotType.WEAPON:
                itemUI.dataSO = InventoryManager.Instance.statsData;
                if(itemUI.dataSO.items[itemUI.Index].itemData != null)
                    GameManager.Instance.playerStates.ChangeWeapon(itemUI.dataSO.items[itemUI.Index].itemData);
                else
                    GameManager.Instance.playerStates.UnEquipWeapon();
                break;
            case SlotType.ACTION:
                itemUI.dataSO = InventoryManager.Instance.actionData;
                break;
        }
        var item = itemUI.dataSO.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData,item.amounts);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem().itemData != null)
        {
            if (itemUI.GetItem().itemData.itemType == ItemType.Consumable && itemUI.GetItem().amounts > 0)
            {
                GameManager.Instance.playerStates.Recover(itemUI.GetItem().itemData.consume.healthPoint);
                itemUI.GetItem().amounts -= 1;
                
                //检测任务更新进度
                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemData.itemName,-1);
            }
        }
        UpdateItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem().itemData != null)
        {
            InventoryManager.Instance.tooltips.SetupTooltips(itemUI.GetItem().itemData);
            InventoryManager.Instance.tooltips.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.tooltips.gameObject.SetActive(false);
    }
}

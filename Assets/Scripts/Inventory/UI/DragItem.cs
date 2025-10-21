using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DragItem : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public ItemUI currentItemUI;
    [FormerlySerializedAs("currSlotUI")] public SlotUI currentSlotUI;
    public SlotUI targetSlotUI;
    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentSlotUI = GetComponentInParent<SlotUI>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //让InventoryManager作为中介保管原有Slot数据
        InventoryManager.Instance.currentData = new InventoryManager.DragData();
        InventoryManager.Instance.currentData.originalHolder = GetComponentInParent<SlotUI>();
        InventoryManager.Instance.currentData.originalParent = (RectTransform)transform.parent;
        //把拖拽图标存放与独立的Canvas中从而使其在最前端显示
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInBagUI(eventData.position) || InventoryManager.Instance.CheckInActionUI(eventData.position)
                || InventoryManager.Instance.CheckInStatsUI(eventData.position))
            {
                if (eventData.pointerEnter.transform.GetComponent<SlotUI>())
                    targetSlotUI = eventData.pointerEnter.transform.GetComponent<SlotUI>();
                else
                    targetSlotUI = eventData.pointerEnter.transform.GetComponentInParent<SlotUI>();
                if(targetSlotUI != InventoryManager.Instance.currentData.originalHolder)
                    switch (targetSlotUI.slotType)
                    {
                        case SlotType.BAG:
                            SwapItem();
                            break;
                        case SlotType.ACTION:
                            if(currentItemUI.dataSO.items[currentItemUI.Index].itemData.itemType == ItemType.Consumable)
                                SwapItem();
                            break;
                        case SlotType.WEAPON:
                            if(currentItemUI.dataSO.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                                SwapItem();
                            break;
                        case SlotType.ARMOR:
                            if(currentItemUI.dataSO.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                                SwapItem();
                            break;
                    }
                
                currentSlotUI.UpdateItem();
                targetSlotUI.UpdateItem();
            }
        }
        
        transform.SetParent(InventoryManager.Instance.currentData.originalParent);
        RectTransform rect = transform as RectTransform;
        rect.offsetMax = -Vector2.one * 5;
        rect.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetSlotUI.itemUI.dataSO.items[targetSlotUI.itemUI.Index];
        var tempItem = currentSlotUI.itemUI.dataSO.items[currentSlotUI.itemUI.Index];

        bool isSame = targetItem.itemData == tempItem.itemData;

        if (isSame && targetItem.itemData.stackable)
        {
            // targetItem.amounts += tempItem.amounts;
            // tempItem.itemData = null;
            // tempItem.amounts = 0;
            targetSlotUI.itemUI.dataSO.items[targetSlotUI.itemUI.Index].amounts += tempItem.amounts;
            currentSlotUI.itemUI.dataSO.items[currentSlotUI.itemUI.Index].itemData = null;
            currentSlotUI.itemUI.dataSO.items[currentSlotUI.itemUI.Index].amounts = 0;
        }
        else
        {
            targetSlotUI.itemUI.dataSO.items[targetSlotUI.itemUI.Index] = tempItem;
            currentSlotUI.itemUI.dataSO.items[currentSlotUI.itemUI.Index] = targetItem;
        }
    }
}

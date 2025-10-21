using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private ItemUI currentItem; 

    private void Awake()
    {
        currentItem = GetComponent<ItemUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestUI.Instance.tooltips.gameObject.SetActive(true);
        QuestUI.Instance.tooltips.SetupTooltips(currentItem.itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.Instance.tooltips.gameObject.SetActive(false);
    }
}

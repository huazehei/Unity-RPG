using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;
    private SlotUI currentSlot;

    private void Awake()
    {
        currentSlot = GetComponent<SlotUI>();
    }

    void Update()
    {
        if(Input.GetKeyDown(actionKey) && currentSlot.itemUI.GetItem().itemData)
            currentSlot.UseItem();
    }
}

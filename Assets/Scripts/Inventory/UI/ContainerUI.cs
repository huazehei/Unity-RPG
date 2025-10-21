using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotUI[] slotUI;

    public void RefreshUI()
    {
        for (int i = 0; i < slotUI.Length; i++)
        {
            slotUI[i].itemUI.Index = i;
            slotUI[i].UpdateItem();
        }
    }
}

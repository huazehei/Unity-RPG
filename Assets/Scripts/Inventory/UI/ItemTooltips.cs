using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltips : MonoBehaviour
{
    public Text itemName;
    public Text itemInfo;

    public RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    public void SetupTooltips(ItemData_SO itemData)
    {
        itemName.text = itemData.itemName;
        itemInfo.text = itemData.discription;
    }

    public void UpdatePosition()
    {
        var mousePos = Input.mousePosition;
        var rectPos = new Vector3[4];
        rect.GetWorldCorners(rectPos);

        float width = rectPos[3].x - rectPos[0].x;
        float height = rectPos[1].y - rectPos[0].y;

        if (mousePos.y < height)
        {
            rect.position = mousePos + height * 0.6f * Vector3.up;
        }
        else if (Screen.width - mousePos.x < width)
        {
            rect.position = mousePos + width * 0.6f * Vector3.left;
        }
        else
        {
            rect.position = mousePos + width * 0.6f * Vector3.right;
        }
    }
}

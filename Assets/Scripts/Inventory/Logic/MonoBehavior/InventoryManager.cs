using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        //存有父类格子数据，父类格子的相对位移信息
        public SlotUI originalHolder;
        public RectTransform originalParent;
    }
    //TODO:还需添加模板用于存储和加载时数据的保存

    [Header("Inventory Data")] 
    public InventoryData_SO bagTempData;
    public InventoryData_SO bagData;
    public InventoryData_SO actionTempData;
    public InventoryData_SO actionData;
    public InventoryData_SO statsTempData;
    public InventoryData_SO statsData;

    [Header("Container Scripts")] 
    public ContainerUI bagUI;
    public ContainerUI actionUI;
    public ContainerUI statsUI;

    [Header("Drag Canvas")] 
    public Canvas dragCanvas;

    public DragData currentData;

    [Header("UI Panel")] 
    public GameObject statsPanel;
    public GameObject inventoryPanel;
    private bool isOpen;

    [Header("Stats UI")] 
    public Text healthText;
    public Text attackText;

    [Header("Item UI")] public ItemTooltips tooltips;

    protected override void Awake()
    {
        base.Awake();
        if (bagTempData != null)
            bagData = bagTempData;
        if (actionTempData != null)
            actionData = actionTempData;
        if (statsTempData != null)
            statsData = statsTempData;
    }

    private void Start()
    {
        LoadData();
        
        bagUI.RefreshUI();
        actionUI.RefreshUI();
        statsUI.RefreshUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            statsPanel.SetActive(isOpen);
            inventoryPanel.SetActive(isOpen);
        }
        UpdateStatsText(GameManager.Instance.playerStates.MaxHealth,GameManager.Instance.playerStates.attackData.minDamage,
            GameManager.Instance.playerStates.attackData.maxDamage);
    }

    public void UpdateStatsText(int health, int min, int max)
    {
        healthText.text = health.ToString();
        attackText.text = min + "-" + max;
    }

#region InventoryItemChangeLogic
//背包栏判断
public bool CheckInBagUI(Vector3 position)
{
    for (int i = 0; i < bagUI.slotUI.Length; i++)
    {
        RectTransform rect = bagUI.slotUI[i].transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(rect, position))
            return true;
    }

    return false;
} 
//状态栏判断
    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotUI.Length; i++)
        {
            RectTransform rect = actionUI.slotUI[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, position))
                return true;
        }

        return false;
    } 
//装备栏判断
    public bool CheckInStatsUI(Vector3 position)
    {
        for (int i = 0; i < statsUI.slotUI.Length; i++)
        {
            RectTransform rect = statsUI.slotUI[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, position))
                return true;
        }

        return false;
    } 
//直接叠加到状态栏中的方法
public void AddItemToInventories(ItemData_SO newItemData, int amount)                
{                                                                                   
        // For stackable items, prioritize stacking in the action bar.                   
       if (newItemData.stackable)                                                       
       {                                                                               
            // Check Action Bar first                                                    
           foreach (var item in actionData.items) 
           {                                                                            
              if (item.itemData == newItemData)                                        
                {                                                                        
                    item.amounts += amount;                                              
                     actionUI.RefreshUI();                                                
                     return;                                                              
                }                                                                       
           }                                                                            
       }                                                                                
                                                                                           
   
                                                         
        bagData.AddItem(newItemData, amount);                                            
        bagUI.RefreshUI();                                                               
    }
#endregion

#region SaveAndLoadInventoryData

    public void SaveData()
    {
        SaveManager.Instance.Save(bagData,bagData.name);
        SaveManager.Instance.Save(actionData,actionData.name);
        SaveManager.Instance.Save(statsData,statsData.name);
    }

    public void LoadData()
    {
        SaveManager.Instance.Load(bagData,bagData.name);
        SaveManager.Instance.Load(actionData,actionData.name);
        SaveManager.Instance.Load(statsData,statsData.name);
    }


#endregion

    //在接受任务时先检测一遍背包中的物品数据
    public void CheckRequireItemInInventory(string requireName)
    {
        foreach (var item in bagData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == requireName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName,item.amounts);
                }
            }
        }
        
        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == requireName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName,item.amounts);
                }
            }
        }
    }
    
    //检测任务提交道具的数据
    public InventoryItem GetBagItem(ItemData_SO data) => bagData.items.Find(
        i => i.itemData == data);

    public InventoryItem GetActionItem(ItemData_SO data) => actionData.items.Find(
        i => i.itemData == data);
}

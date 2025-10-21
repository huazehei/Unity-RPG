using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemPickup : MonoBehaviour
{
    [FormerlySerializedAs("swordData")] public ItemData_SO pickItemData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //该方法使得可堆叠物品即便在快捷栏Action也能直接叠加
            InventoryManager.Instance.AddItemToInventories(pickItemData,pickItemData.itemAmounts);
            
            //以下为默认逻辑，即只在背包栏叠加
            // InventoryManager.Instance.bagData.AddItem(pickItemData,pickItemData.itemAmounts);
            // InventoryManager.Instance.bagUI.RefreshUI();
            
            //检测任务
            QuestManager.Instance.UpdateQuestProgress(pickItemData.itemName,pickItemData.itemAmounts);
            
            //装备武器
            //GameManager.Instance.playerStates.EquipWeapon(swordData);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

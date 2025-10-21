using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

[CreateAssetMenu(fileName = "New Quest",menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class Require
    {
        public string requireName;
        public int requireAmount;
        public int currentAmount;
    }
    public string questName;
    [TextArea]
    public string description;

    public bool isStarted;
    public bool isComplete;
    public bool isFinished;

    public List<Require> requires = new();
    public List<InventoryItem> rewards = new();
    
    //检测任务是否已完成
    public void CheackQuestProgress()
    {
        var completedRequire = requires.Where(r => r.requireAmount <= r.currentAmount);

        isComplete = completedRequire.Count() == requires.Count;
        if(isComplete)
            Debug.Log("任务完成");
    }
    
    //获取任务需求的名字列表
    public List<string> RequireNameList()
    {
        List<string> requireNames = new();
        foreach (var require in requires)
        {
            requireNames.Add(require.requireName);
        }
        return requireNames;
    }

    public void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.amounts < 0)
            {
                int requireAmount = Mathf.Abs(reward.amounts);
                var bagItem = InventoryManager.Instance.GetBagItem(reward.itemData);
                var actionItem = InventoryManager.Instance.GetActionItem(reward.itemData);
                if (bagItem != null)
                {
                    if (bagItem.amounts <= requireAmount)
                    {
                        requireAmount -= bagItem.amounts;
                        bagItem.amounts = 0;
                        if(actionItem != null)
                            actionItem.amounts -= requireAmount;
                    }
                    else
                    {
                        bagItem.amounts -= requireAmount;
                    }
                }
                else
                {
                    actionItem.amounts -= requireAmount;
                }
            }
            else
            {
                InventoryManager.Instance.bagData.AddItem(reward.itemData,reward.amounts);
            }
            InventoryManager.Instance.bagUI.RefreshUI();
            InventoryManager.Instance.actionUI.RefreshUI();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Quest Element")] 
    public GameObject questPanel;
    public ItemTooltips tooltips;
    private bool isOpen;

    [Header("Quest Name Button")] 
    public RectTransform listRect;
    public QuestNameButton questNameButton;
    
    [Header("Text Content")] 
    public Text questContent;

    [Header("Quest Requirement")] 
    public RectTransform requirementListRect;
    public QuestRequirement requirement;

    [Header("Quest Reward")]  
    public RectTransform rewardListRect;
    public ItemUI rewardItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //开闭面板，每次开启将内容置空
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContent.text = "";
            //清除原有面板
            SetupQuestList();
            
            if(!isOpen)
                tooltips.gameObject.SetActive(false);
        }
    }
    //更新按钮列表
    public void SetupQuestList()
    {
        foreach (Transform button in listRect)
        {
            Destroy(button.gameObject);
        }

        foreach (Transform reward in rewardListRect)
        {
            Destroy(reward.gameObject);
        }

        foreach (Transform require in requirementListRect)
        {
            Destroy(require.gameObject);
        }

        foreach (var task in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, listRect);
            newTask.SetupButtonName(task.currentData);
            newTask.questContent = questContent;
        }
    }
    //更新任务需求列表
    public void UpdateRequireList(QuestData_SO data)
    {
        foreach (Transform require in requirementListRect)
        {
            Destroy(require.gameObject);
        }

        foreach (var require in data.requires)
        {
            var newRequire = Instantiate(requirement, requirementListRect);
            if(data.isFinished)
                newRequire.SetupRequirement(require.requireName);
            else
                newRequire.SetupRequirement(require.requireName,require.currentAmount,require.requireAmount);
        }
        
    }
    //更新任务奖励列表与显示UI
    public void SetupRewardItem(ItemData_SO itemData,int itemAmount)
    {
        var reward = Instantiate(rewardItem, rewardListRect);
        reward.SetupItemUI(itemData,itemAmount);
    }
}

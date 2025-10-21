using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questName;
    public QuestData_SO questData;
    public Text questContent;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    public void SetupButtonName(QuestData_SO data)
    {
        questData = data;
        if (questData.isComplete)
            questName.text = questData.questName + " (已完成)";
        else
            questName.text = questData.questName;
    }

    public void UpdateQuestContent()
    {
        questContent.text = questData.description;
        QuestUI.Instance.UpdateRequireList(questData);
        
        //遍历销毁上一个任务的奖励UI
        foreach (Transform item in QuestUI.Instance.rewardListRect)
        {
            Destroy(item.gameObject);
        }
        
        //遍历显示奖励UI
        foreach (var item in questData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(item.itemData,item.amounts);
        }
    }
}

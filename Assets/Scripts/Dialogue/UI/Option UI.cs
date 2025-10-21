using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    private Button optionButt;
    private DialoguePiece currentPiece;
    private string targetID;
    private bool takeQuest;

    private void Awake()
    {
        optionButt = GetComponent<Button>();
        optionButt.onClick.AddListener(ClickOption);
    }

    public void UpdateOption(DialoguePiece piece,DialogueOption option)
    {
        currentPiece = piece;
        optionText.text = option.optionText;
        targetID = option.targetID;
        takeQuest = option.takeQuest;
    }

    public void ClickOption()
    {
        if (currentPiece.questData != null)
        {
            var taskData = new QuestManager.QuestTask()
            {
                currentData = Instantiate(currentPiece.questData)
            };
        
            if (takeQuest)
            {
                if (QuestManager.Instance.HaveQuest(taskData.currentData))
                {
                    //判断任务是否完成并给予奖励
                    if (QuestManager.Instance.GetTask(taskData.currentData).IsComplete)
                    {
                        taskData.currentData.GiveRewards();
                        QuestManager.Instance.GetTask(taskData.currentData).IsFinished = true;
                    }
                }
                else
                {
                    QuestManager.Instance.tasks.Add(taskData);
                    QuestManager.Instance.GetTask(taskData.currentData).IsStarted = true;
                    
                    //在接受任务后还需检测背包中的数据
                    foreach (var requireName in taskData.currentData.RequireNameList())
                    {
                        InventoryManager.Instance.CheckRequireItemInInventory(requireName);
                    }
                }
            }
        }
        if (targetID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueMap[targetID]);
        }
    }
}

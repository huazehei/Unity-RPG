using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    private DialogueController contoller;
    private QuestData_SO currentQuest;

    public DialogueDataSO startDialogue;
    public DialogueDataSO progressDialogue;
    public DialogueDataSO completeDialogue;
    public DialogueDataSO finishedDialogue;
    
    public bool IsStart
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;
            else
                return false;
        }
    }
    
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;
            else
                return false;
        }
    }
    
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;
            else
                return false;
        }
    }
    private void Awake()
    {
        contoller = GetComponent<DialogueController>();
    }

    private void Start()
    {
        contoller.currentData = startDialogue;
        currentQuest = contoller.currentData.GetQuest();
    }

    private void Update()
    {
        if (IsStart)
        {
            if (IsComplete)
                contoller.currentData = completeDialogue;
            else
                contoller.currentData = progressDialogue;
        }

        if (IsFinished)
            contoller.currentData = finishedDialogue;
    }
}

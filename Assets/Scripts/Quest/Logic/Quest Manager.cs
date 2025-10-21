using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO currentData;
        public bool IsStarted
        {
            get => currentData.isStarted;
            set => currentData.isStarted = value;
        }
        public bool IsComplete
        {
            get => currentData.isComplete;
            set => currentData.isComplete = value;
        }
        public bool IsFinished
        {
            get => currentData.isFinished;
            set => currentData.isFinished = value;
        }
    }

    public List<QuestTask> tasks = new();

    private void Start()
    {
        LoadQuestSystem();
    }

    //保存任务数据
    public void SaveQuestSystem()
    {
        PlayerPrefs.SetInt("QuestCount",tasks.Count);
        for (int i = 0; i < tasks.Count; i++)
        {
            SaveManager.Instance.Save(tasks[i].currentData,"task" + i);
        }
    }
    
    //加载任务数据
    public void LoadQuestSystem()
    {
        var questCount = PlayerPrefs.GetInt("QuestCount");
        for (int i = 0; i < questCount; i++)
        {
            var newQuestData = ScriptableObject.CreateInstance<QuestData_SO>();
            SaveManager.Instance.Load(newQuestData, "task" + i);
            tasks.Add(new QuestTask{currentData = newQuestData});
        }
    }
    
    public bool HaveQuest(QuestData_SO data)
    {
        if (data != null) return tasks.Any(q => q.currentData.questName == data.questName);
        return false;
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.currentData.questName == data.questName);
    }
    
    //检测任务进度
    public void UpdateQuestProgress(string requireName, int amount)
    {
        foreach (var require in QuestManager.Instance.tasks)
        {
            if (require.IsFinished)
                continue;
            //更新任务进度
            var requirement = require.currentData.requires.Find(q => q.requireName == requireName);
            if(requirement != null)
                requirement.currentAmount = Math.Min(requirement.currentAmount + amount,requirement.requireAmount);
            
            //检测是否已完成
            require.currentData.CheackQuestProgress();
        }
    }
}

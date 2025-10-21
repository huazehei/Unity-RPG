using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Dialogue",menuName = "Dialogue/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new();
    public Dictionary<string, DialoguePiece> dialogueMap = new();
    
#if UNITY_EDITOR    
    private void OnValidate()
    {
        foreach (var dialoguePiece in dialoguePieces)
        {
            if(!dialogueMap.ContainsKey(dialoguePiece.ID))
                dialogueMap.Add(dialoguePiece.ID,dialoguePiece);
        }
    }
#endif   
    
    //获取当前对话列表中的任务数据
    public QuestData_SO GetQuest()
    {
        QuestData_SO data = null;
        foreach (var piece in dialoguePieces)
        {
            if (piece.questData != null)
                data = piece.questData;
        }

        return data;
    }
}

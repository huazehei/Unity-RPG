using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueDataSO currentData;
    public bool canTalk = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null)
            canTalk = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            DialogueUI.Instance.dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (canTalk && Input.GetMouseButtonDown(1))
            OpenDialogue(); 
    }

    public void OpenDialogue()
    {
        DialogueUI.Instance.UpdateDialogueData(currentData);
        DialogueUI.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);
    }
}

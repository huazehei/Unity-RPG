using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Element")]
    public Image icon;
    public Text dialogueText;
    public Button nextButton;
    public GameObject dialoguePanel;
    
    [Header("Option")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;
    
    [Header("Data")] 
    public DialogueDataSO currentData;
    public int currentIndex;
    
    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    public void ContinueDialogue()
    {
        if (currentIndex < currentData.dialoguePieces.Count)
        {
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void UpdateDialogueData(DialogueDataSO dataSo)
    {
        currentData = dataSo;
        currentIndex = 0;
    }

    public void UpdateMainDialogue(DialoguePiece piece)
    {
        //显示对话图标
        dialoguePanel.SetActive(true);
        currentIndex++;
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }
        
        //显示对话文本
        dialogueText.text = "";
        //dialogueText.text = piece.text;
        dialogueText.DOText(piece.text, 1f);
        
        //显示下句对话按钮
        if (piece.dialogueOptions.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        
        //创建对话对应的选项
        CreateOptions(piece);
    }

    private void CreateOptions(DialoguePiece piece)
    {
        if (optionPanel.childCount > 0)
        {
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        for(int i = 0;i<piece.dialogueOptions.Count;i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.UpdateOption(piece,piece.dialogueOptions[i]);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requiredName;
    private Text progress;

    private void Awake()
    {
        requiredName = GetComponent<Text>();
        progress = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetupRequirement(string requireName, int currentNum, int targetNum)
    {
        requiredName.text = requireName;
        progress.text = currentNum.ToString() + " / " + targetNum.ToString();
    }

    public void SetupRequirement(string requireName)
    {
        requiredName.text = requireName;
        progress.text = "已完成";
        requiredName.color = Color.gray;
        progress.color = Color.gray;
    }
}

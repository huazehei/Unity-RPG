using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private Text level;
    private Image healthSlider;
    private Image expSlider;

    private void Awake()
    {
        level = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if (GameManager.Instance.playerStates == null || GameManager.Instance.playerStates.characterData
            == null) return;
        level.text = "Level " + GameManager.Instance.playerStates.characterData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();
    }

    private void UpdateHealth()
    {
        float healthPercent = (float)GameManager.Instance.playerStates.CurrentHealth /
                              GameManager.Instance.playerStates.MaxHealth;
        
        healthSlider.fillAmount = healthPercent;
        
    }

    private void UpdateExp()
    {
        float expPercent = (float)GameManager.Instance.playerStates.characterData.currentExp /
                           GameManager.Instance.playerStates.characterData.upgradeExp;
        expSlider.fillAmount = expPercent;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    //血条的预制体
    public GameObject healthBarPrefab;
    public Transform barPoint;
    //控制血条是否持续可见及可见时间
    public bool isAlwaysVisible;
    public float visibleTime;
    private float timeLeft;

    //血量
    private Image healthSlider;
    //实际血条
    private Transform UIBar;
    //需要摄像机位置来保证血条一直面向摄像机
    private Transform cam;
    private CharacterStats characterStats;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterStats.UpdateHealthToAttack += UpdateHealth;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (var canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthBarPrefab, canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(isAlwaysVisible);
            }
        }
    }

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIBar.gameObject);
        UIBar.gameObject.SetActive(true);
        timeLeft = visibleTime;
        float healthPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = healthPercent;
    }

    private void LateUpdate()
    {
        if (UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;
        }

        if (timeLeft <= 0 && !isAlwaysVisible)
        {
            UIBar.gameObject.SetActive(false);
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }
}

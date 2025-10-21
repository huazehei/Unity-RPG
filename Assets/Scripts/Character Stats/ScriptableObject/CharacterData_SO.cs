using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")] 
    public int maxHealth;
    public int currentHealth;
    public int maxDefence;
    public int currentDefence;
    [Header("Kill Score")]
    public int score;

    [Header("Level System")] 
    public int currentLevel;
    public int maxLevel;
    public int currentExp;
    public int upgradeExp;

    public float upgradeBuff;
    public float levelMultipler
    {
        get => 1 + (currentLevel - 1) * upgradeBuff;
    }

    public void UpgradeLevel(int score)
    {
        currentExp += score;
        if (currentExp > upgradeExp)
        {
            
            LevelUp();
        }
    }

    public void LevelUp()
    {
        //所有关于提升等级数据的方法
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        upgradeExp = (int)(upgradeExp * levelMultipler);
        currentExp -= upgradeExp;
        maxHealth = (int)(maxHealth * levelMultipler);
        currentHealth = maxHealth;
        maxDefence = (int)(maxDefence * levelMultipler);
        currentDefence = maxDefence;
        Debug.Log("Level Up: " + currentLevel);
    }
}

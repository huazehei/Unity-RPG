using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthToAttack;
    public CharacterData_SO tempData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    private AttackData_SO baseaAttackData;
    private RuntimeAnimatorController baseAnimator;
    [HideInInspector] public bool isCritical;
    [Header("Weapon")] 
    public Transform weaponTrans;
    [Header("Armour")]
    public Transform sheildTrans;
    #region Read from Data_SO

    void Awake()
    {
        if (tempData != null)
            characterData = Instantiate(tempData);
        baseaAttackData = Instantiate(attackData);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth;else return 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth;else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int MaxDefence
    {
        get { if (characterData != null) return characterData.maxDefence;else return 0; }
        set { characterData.maxDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence;else return 0; }
        set { characterData.currentDefence = value; }
    }
    
    #endregion

    #region Combat

    public void TakeDamage(CharacterStats attackStats,CharacterStats defenderStats)
    {
        int damage = Mathf.Max(attackStats.CurrentDamage() - defenderStats.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        
        //暴击播放受击动画
        if (attackStats.isCritical)
        {
            defenderStats.GetComponent<Animator>().SetTrigger("Hit");
        }
        //TODO:UI Update
        UpdateHealthToAttack?.Invoke(CurrentHealth, MaxHealth);
        //TODO:经验值 Update
        if (CurrentHealth <= 0)
            attackStats.characterData.UpgradeLevel(characterData.score);

    }

    public void TakeDamage(int damage, CharacterStats defender)
    {
        damage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        UpdateHealthToAttack?.Invoke(CurrentHealth, MaxHealth);
        if(CurrentHealth<=0)
            GameManager.Instance.playerStates.characterData.UpgradeLevel(characterData.score);
    }

    private int CurrentDamage()
    {
        float coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("暴击" + coreDamage);
        }

        return (int)coreDamage;
    }
    

    #endregion

    #region WeaponAndArmour

    public void ChangeArmour(ItemData_SO armour)
    {
        UnEquipArmour();
        EquipArmour(armour);
    }

    public void EquipArmour(ItemData_SO armour)
    {
        if (armour.weaponPrefab != null)
        {
            Instantiate(armour.weaponPrefab, sheildTrans);
        }
        GetComponent<Animator>().runtimeAnimatorController = armour.weaponController;
    }

    public void UnEquipArmour()
    {
        if (sheildTrans.transform.childCount != 0)
        {
            for (int i = 0; i < sheildTrans.transform.childCount; i++)
            {
                Destroy(sheildTrans.transform.GetChild(i).gameObject);
            }
        }

        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }
    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }

    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab, weaponTrans);
        }
        //更新人物属性
        attackData.ApplyWeaponData(weapon.weaponData);
        //更改人物动画
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponController;
    }

    public void UnEquipWeapon()
    {
        if (weaponTrans.transform.childCount != 0)
        {
            for (int i = 0; i < weaponTrans.transform.childCount; i++)
            {
                Destroy(weaponTrans.transform.GetChild(i).gameObject);
            }
        }
        
        //还原人物攻击属性
        attackData.ApplyWeaponData(baseaAttackData);
        //更改人物动画
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }

    #endregion
    
#region UpdateData

    public void Recover(int num)
    {
        if (CurrentHealth + num <= MaxHealth)
            CurrentHealth += num;
        else
            CurrentHealth = MaxHealth;
    }
#endregion    
}

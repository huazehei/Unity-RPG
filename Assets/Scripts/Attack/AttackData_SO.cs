using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Attack",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange,skillRange,coolDown,criticalMultiplier,criticalChance;
    public int minDamage, maxDamage;

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        if (weapon == null)
            return;
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Consumable, Weapon , Armor
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public bool stackable;
    public int itemAmounts;
    public Sprite itemIcon;
    [TextArea(2,5)] 
    public string discription = "";

    [Header("ConsumableItem")] 
    public ConsumableData_SO consume;
    
    [Header("Weapon")] 
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
    public RuntimeAnimatorController weaponController;
}

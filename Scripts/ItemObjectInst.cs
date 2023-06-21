using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    HealthPotion,
    Chest,
    Default,
    Ruby,
    Emerald,
    WaterBottle,
    StaminaPotion,
    ManaPotion,
    Sapphire,
    Boots,
    Sword,
    Wand
}

public enum Attributes
{
    Health,
    Mana,
    Attack,
    Speed,
    Stamina
}
[CreateAssetMenu(fileName="New Item",menuName ="Inventory System/Items/item")]
public class ItemObjectInst : ScriptableObject
{

    public bool stackable;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15, 20)] public string description;
    public ItemInst data = new ItemInst();

    public ItemInst CreateItem()
    {
        ItemInst newItem = new ItemInst(this);
        return newItem;
    }
}

[System.Serializable]
public class ItemInst
{
    public string name;
    public int Id=-1;
    public ItemType _type;
    public ItemBuff[] buffs;
    public ItemInst()
    {
        name = "";
        Id = -1;
    }
    public ItemInst(ItemObjectInst item)
    {
        name = item.name;
        Id = item.data.Id;
        _type = item.type;
        buffs = new ItemBuff[item.data.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {
            
            buffs[i] = new ItemBuff(item.data.buffs[i].value);
            buffs[i].attribute = item.data.buffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff:IModifier
{
    public Attributes attribute;
    public int value;
    public ItemBuff(int _value)
    {
        value = _value;
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }
}

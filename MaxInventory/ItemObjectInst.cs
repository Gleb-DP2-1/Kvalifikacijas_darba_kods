using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObjectInst : ScriptableObject
{
    public enum ItemType
    {
        Potion,
        Equipment,
        Default
    }
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)] public string description;
}

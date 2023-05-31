using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Potion Obj", menuName = "Inventory System/Items/Potion")]
public class PotionObject : ItemObjectInst
{
    public int restoreHealthValue;
    private void Awake()
    {
        type = ItemType.Potion;
    }
}

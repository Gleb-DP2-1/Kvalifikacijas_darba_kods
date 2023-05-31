using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment Obj", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObjectInst
{
    public int atkValue;
    public int defValue;
    private void Awake()
    {
        type = ItemType.Equipment;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Default Obj", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObjectInst
{
    private void Awake()
    {
        type = ItemType.Default;
    }
}

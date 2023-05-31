using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMax : MonoBehaviour
{
    public ItemObjectInst item;

    public InventoryObject inventory;
   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerInventory>();
        if (item)
        {
            inventory.AddItem(item, 1);
            Destroy(this.gameObject);
        }
    }*/
}

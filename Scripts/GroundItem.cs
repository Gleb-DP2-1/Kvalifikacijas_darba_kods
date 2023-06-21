using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour,ISerializationCallbackReceiver
{
    public ItemObjectInst item;

    public InventoryObject inventory;

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }
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

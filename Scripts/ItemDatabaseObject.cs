using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Item Database",menuName ="Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObjectInst[] ItemsObjects;
    
   // public Dictionary<int,ItemObjectInst> GetItem = new Dictionary< int, ItemObjectInst>();
   [ContextMenu("Update ID's")]
   public void UpdateID()
    {
        for (int i = 0; i < ItemsObjects.Length; i++)
        {
            if (ItemsObjects[i].data.Id != i)
                ItemsObjects[i].data.Id = i;
            // GetItem.Add(i, Items[i]);
        }
    }

    public void OnAfterDeserialize()
    {

        UpdateID();
       
    }

    public void OnBeforeSerialize()
    {
       // GetItem = new Dictionary<int, ItemObjectInst>();
    }
}

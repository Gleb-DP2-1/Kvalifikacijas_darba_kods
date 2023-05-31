using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int X_offset;
    public int Column_Count;
    public int Y_offset;
    private bool flag = false;
    public PlayerHealth health;

    public GameObject InventoryTab;

    Dictionary<InventorySlot, GameObject> items = new Dictionary<InventorySlot, GameObject>();
    void Start()
    {
       
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ItemMax temp;
            for (int i = 0; i < inventory.Container.Count; i++)
            {
                GameObject temp1 = items[inventory.Container[i]];
                temp = temp1.GetComponent<ItemMax>();
                if (temp != null)
                {
                    if (temp.item.type == ItemObjectInst.ItemType.Potion)
                    {
                        inventory.Container[i].amount--;
                        break;
                    }
                }
                    
                      }
        }

        
            UpdateDisplay();
        
      
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (items.ContainsKey(inventory.Container[i]))
            {
                items[inventory.Container[i]].GetComponentInChildren<Text>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<Text>().text = inventory.Container[i].amount.ToString("n0");
                items.Add(inventory.Container[i], obj);
            }
        }
    }
    public void CreateDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<Text>().text = inventory.Container[i].amount.ToString("n0");
            items.Add(inventory.Container[i], obj);


        }
        flag = true;

    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_offset * (i % Column_Count), (-Y_offset * (i / Column_Count)), 0f);
    }
    
}

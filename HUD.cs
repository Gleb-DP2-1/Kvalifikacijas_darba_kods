using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject InventoryTab;
    // Update is called once per frame
    void Update()
    {
        ManageInventoryBar();
    }


    public void ManageInventoryBar()
    {
        if (InventoryTab != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                InventoryTab.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                InventoryTab.SetActive(false);
            }
        }
    }
}

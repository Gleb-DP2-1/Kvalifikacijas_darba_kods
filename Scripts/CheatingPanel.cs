using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatingPanel : MonoBehaviour
{
   
    [SerializeField] InventoryObject inventory;
    [SerializeField] ItemInst[] CheatingItems;
    public bool inLevel;
    [SerializeField] Text cheatText;
    float alpha = 0f;
    static bool CheatsEnabled;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.O)&& Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.P))
        {
            alpha = 1;
            cheatText.text = "Admin rights have been activated";
            CheatsEnabled = true;
        }

        if (alpha > 0)
        {
            alpha -= 0.2f * Time.deltaTime;
            cheatText.color = new Color(255, 255, 255, alpha);
        }
        else
        {
            alpha = 0;
        }
        if (CheatsEnabled)
        {
            SetUpCheats();
        }
    }



    public void SetUpCheats()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            for(int i = 0; i < CheatingItems.Length; i++)
            {
                inventory.AddItem(CheatingItems[i], 999);
                
            }
            PlayerPrefs.SetInt("coins", 10000);
            cheatText.text = "All items cheat activated";
            alpha = 1;
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            if (inLevel)
            {
                GetComponentInParent<HUD>().condition = 1;
                alpha = 1;
                cheatText.text = "Skipping the level cheat activated";
            }
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            RealMovement.Instance.GodMode = true;
            alpha = 1;
            cheatText.text = "God Mode cheat activated";
        }
    }
}

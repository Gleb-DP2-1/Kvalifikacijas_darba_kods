using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;


public class Shop : MonoBehaviour
{
    public string objectName;
    public int price, access;
    public string item;
    public GameObject block;
    public Text objectPrice, CoinCount;
    public InventoryObject inventory;
    public ItemMax item1;

    void Awake()
    {   
        AccessUpdate();
    }

    void AccessUpdate()
    {
       // access = PlayerPrefs.GetInt(objectName + "Access");
        objectPrice.text = price.ToString();
        CoinCount.text = PlayerPrefs.GetInt("coins").ToString();

       /* if(access == 1)
        {
            block.SetActive(true);
            objectPrice.gameObject.SetActive(true);
        }*/
    }

    public void PayButton()
    {
        int coins = PlayerPrefs.GetInt("coins");

        if (access == 0)
        {
            if(coins >= price)
            {
               // PlayerPrefs.SetInt(objectName  + "Access", 1);
                PlayerPrefs.SetInt("coins", coins - price);
                AccessUpdate();
                inventory.AddItem(item1.item, 1);


            }
        }
        else 
        {
            //SceneManager.LoadScene(level+2); 
        }

    }
    public void GoBack()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

}   
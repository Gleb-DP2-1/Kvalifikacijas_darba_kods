using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{   
    public string bonusName;
    public Text coinText;
    //public string itemName;
    public Text itemText;

    void Awake()
    {
        if (coinText != null)
        {
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        }
         if (itemText != null)
        {
           itemText.text = PlayerPrefs.GetInt("items").ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            switch (bonusName)
            {
                case "coin":
                    int coins = PlayerPrefs.GetInt("coins");
                    PlayerPrefs.SetInt("coins", coins + 1);
                    if (coinText != null)
                    {
                        coinText.text = (coins + 1).ToString();
                    }
                    Destroy(gameObject);
                    break;
                case "item":
                    int items = PlayerPrefs.GetInt("items");
                    PlayerPrefs.SetInt("items", items + 1);
                    if (itemText != null)
                    {
                        itemText.text = (items + 1).ToString();
                    }
                    Destroy(gameObject);
                    break;

            }
        }
    }
}
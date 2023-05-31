using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class Items
{
    public int _id;
    public string _name;
    public string _description;
    public int _price;
}

public  class GameData : MonoBehaviour
{
    public static GameData instance;
    public static Items[] _items = new Items[12];

    private  void Awake()
    {
        // Populate the items array manually
        _items[0] = new Items { _id = 0, _name = "Helm", _description = "Simple Armor", _price = 10 };
        //_items[1] = new Items { _id = 1, _name = "Item 1", _description = "This is item 1", _price = 15 };
        // Add the remaining items...

        // Access and print the data for each item
        foreach (Items item in _items)
        {
            Debug.Log("ID: " + item._id + ", Name: " + item._name + ", Description: " + item._description + ", Price: " + item._price);
        }
    }
}
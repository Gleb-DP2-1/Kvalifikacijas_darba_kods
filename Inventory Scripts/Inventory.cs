using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;


    public List<Item> items = new List<Item>();
    // Add any other inventory-related variables or properties

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        // Handle any inventory-related actions, UI updates, etc.
    }

    public void GoBack()
    {

       
    }
}


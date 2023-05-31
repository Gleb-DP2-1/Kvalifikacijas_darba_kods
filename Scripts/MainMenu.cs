using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +4); 
    }

    public void ExitGame()
    {
        Debug.Log("Game Exit");
        Application.Quit();
    }

    public void Shop()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void InventoryOpen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
}

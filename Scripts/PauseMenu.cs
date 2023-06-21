using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    HUD hud;
    [SerializeField]GameObject PausePanel;
    void Start()
    {
        hud = GetComponent<HUD>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && hud.condition == 0)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
}

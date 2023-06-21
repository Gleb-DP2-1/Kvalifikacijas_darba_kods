using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public bool FadeInFlag = false;
    public int currentScene;
    float alpha = 1f;
       public Image FadeIn;

    private void Update()
    {
        if (FadeInFlag)
        {
            FadeIn.gameObject.SetActive(true);
            alpha += 1f * Time.deltaTime;
            FadeIn.color = new Color(0, 0, 0, alpha);
        }
      
    }
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

    IEnumerator loadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        //MainMenuPanel.SetActive(false);
        //LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
           // LoadingBar.transform.localScale = new Vector3(progressValue * StartLocalScale, LoadingBar.transform.localScale.y, LoadingBar.transform.localScale.z);
            yield return null;
        }
    }

    public void StartGame()
    {
        FadeInFlag = true;
    }
}

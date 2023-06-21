using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayScreenUI : MonoBehaviour
{

    public AudioMixer audioMixer;
    [SerializeField]private GameObject prevScreen;
    public GameObject MainMenuPanel;
    public GameObject PlayMenuPanel;
    [SerializeField] Text coinSlot;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] GameObject LoadingBar;
    [SerializeField] GameObject OptionPanel;
    float StartLocalScale;
    [SerializeField] InventoryObject inventory;
    [SerializeField] InventoryObject equipment;
    [SerializeField] GameObject startButton;
    float currentVolume;

    public int chosenLevel=-1;
    [TextArea(15, 20)]
    public string[] levelDescriptions;
    public Text DifficultyNotice;
    public Text LvlDescText;


    int coins;
    // Start is called before the first frame update
    void Start()
    {
        inventory.Load();
        equipment.Load();
        Time.timeScale = 1f;
        coins = PlayerPrefs.GetInt("coins");
        StartLocalScale = LoadingBar.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        coins = PlayerPrefs.GetInt("coins");
        coinSlot.text = coins.ToString();
    }

    public void ChangeScreen(GameObject nextScreen)
    {
        if (prevScreen != null)
        {
            prevScreen.SetActive(false);
        }
        nextScreen.SetActive(true);
        prevScreen = nextScreen;
    }

    public void Back()
    {
        MainMenuPanel.SetActive(true);
        PlayMenuPanel.SetActive(false);
    }

    public void OpenOptions()
    {
        MainMenuPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }
    public void CloseOptions()
    {
        MainMenuPanel.SetActive(true);
        OptionPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
        currentVolume = volume;
    }

    public void In(bool reset)
    {
        if (reset)
        {
            inventory.Clear();
            equipment.Clear();
            PlayerPrefs.SetInt("coins", 0);
            coins = 0;
            PlayerPrefs.SetFloat("Difficulty1", 1);
            PlayerPrefs.SetFloat("Difficulty2", 1);
            PlayerPrefs.SetFloat("Difficulty3", 1);
            PlayerPrefs.SetFloat("Difficulty4", 1);
        }
        MainMenuPanel.SetActive(false);
        PlayMenuPanel.SetActive(true);
    }

    IEnumerator loadSceneAsync(int sceneId)
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        MainMenuPanel.SetActive(false);
        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBar.transform.localScale = new Vector3(progressValue * StartLocalScale, LoadingBar.transform.localScale.y, LoadingBar.transform.localScale.z);
            yield return null;
        }
    }
    public void ChooseLevelSlot(int slot)
    {
        chosenLevel = slot;
        LvlDescText.text = levelDescriptions[chosenLevel];
        float multiplier=0;
        startButton.SetActive(true);
        switch (slot)
        {
            case 0:
                multiplier = PlayerPrefs.GetFloat("Difficulty1");
                break;
            case 1:
                multiplier = PlayerPrefs.GetFloat("Difficulty2");
                break;
            case 2:
                multiplier = PlayerPrefs.GetFloat("Difficulty3");
                break;
            case 3:
                multiplier = PlayerPrefs.GetFloat("Difficulty4");
                break;
            default:
                multiplier = 1;
                break;
        }
        DifficultyNotice.text = "Your current Difficulty multiplier is: " + multiplier.ToString() + "\n The larger the number is the stronger enemies are and the less time before bombs drop from the sky.";
    }
    public void LoadScene()
    {
        inventory.Save();
        equipment.Save();
        StartCoroutine(loadSceneAsync(chosenLevel+2));
    }

    public void Quit()
    {
        Application.Quit();
    }
    private void OnApplicationQuit()
    {
        inventory.Save();
        equipment.Save();
    }
}

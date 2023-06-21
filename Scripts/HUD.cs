using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject InventoryTab;
    Level1Manager manager;

    [SerializeField]Text EnemyCount;
    [SerializeField] Text Timer;
    [SerializeField]InventoryObject inventory;
    [SerializeField] InventoryObject equipment;
    [SerializeField] Animator[] animators;
    public GameObject winScreen;
    [SerializeField] Cinemachine.CinemachineVirtualCamera cam;

    float alpha = 0f;

    public GameObject deathScreen;

    bool flag = false;

    [SerializeField]Image FadeOutBlackScreen;
    [SerializeField] GameObject MainUI;
    bool FadeIn = false;
    [SerializeField]public int CurrentLvl;

    bool multiplierCalculationFlag = false;



    public int condition = 0; //-1 = lose    1=win


    private void Start()
    {
 
        FadeOutBlackScreen.color = new Color(0, 0, 0, alpha);
        Time.timeScale = 1f;
    }
    // Update is called once per frame
    void Update()
    {
      
        ManageInventoryBar();
        if (manager.seconds>0&&CurrentLvl!=3)
        {
            Timer.text = Mathf.RoundToInt(manager.seconds).ToString();
        }

        if (RealMovement.Instance.dead)
        {
            condition = -1;
            RealMovement.Instance.Freeze(true);
            RealMovement.Instance.spriteRenderer.sprite = null;
            StartSequence();
           
        }
        
        EnemyCount.text = manager.count.ToString();
        if (condition == 1)
        {
            StartSequence();

        }
      

        if (FadeIn && alpha < 1f)
        {
            alpha += 1f * Time.deltaTime;
            FadeOutBlackScreen.color = new Color(0, 0, 0, alpha);
        } else if (alpha >= 1f)
        {
            alpha = 1;
            if (condition == 1&&!multiplierCalculationFlag) {
                float newMultiplier = 0;
                multiplierCalculationFlag = true;
                switch (CurrentLvl)
                {
                    case 1:
                        newMultiplier = PlayerPrefs.GetFloat("Difficulty1");
                        PlayerPrefs.SetFloat("Difficulty1", newMultiplier + 1.5f);
                        break;
                    case 2:
                        newMultiplier = PlayerPrefs.GetFloat("Difficulty2");
                        PlayerPrefs.SetFloat("Difficulty2", newMultiplier + 1.5f);
                        break;
                    case 3:
                        newMultiplier = PlayerPrefs.GetFloat("Difficulty3");
                        PlayerPrefs.SetFloat("Difficulty3", newMultiplier + 1.5f);
                        break;
                    case 4:
                        newMultiplier = PlayerPrefs.GetFloat("Difficulty4");
                        PlayerPrefs.SetFloat("Difficulty4", newMultiplier + 1.5f);
                        break;
                }
              
                inventory.Save();
                equipment.Save();
                winScreen.SetActive(true);
            }else if (condition == -1)
            {
                inventory.Clear();
                equipment.Clear();
                deathScreen.SetActive(true);
            }
        }
    }

    public void StartSequence()
    {
        cam.m_Lens.OrthographicSize = 5f;
        MainUI.SetActive(false);
        Cursor.visible = true;
       
        Time.timeScale = 0.4f;
        RealMovement.Instance.GodMode = true;
        FadeIn = true;

    }


    public void ManageInventoryBar()
    {
        if (InventoryTab != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!flag)
                {
                    
                    for (int i = 0; i < animators.Length; i++)
                    {
                        animators[i].SetBool("Closed", false);
                    }
                    Time.timeScale = 0f;
                  //  InventoryTab.SetActive(true);
                    Cursor.visible = true;
                    flag = true;
                }
                else
                {
                    for (int i = 0; i < animators.Length; i++)
                    {
                        animators[i].SetBool("Closed", true);
                    }
                    Time.timeScale = 1f;
                   // InventoryTab.SetActive(false);
                    Cursor.visible = false;
                    flag = false;
                }
               
            }
           
        }
    }

    private void Awake()
    {
        manager = FindObjectOfType<Level1Manager>();
    }
}

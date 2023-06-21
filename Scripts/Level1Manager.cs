using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    RealEnemyBase[] enemies;
    List<RealEnemyBase> enemyList = new List<RealEnemyBase>();
    public int count;
    public GameObject BombSpawnPoint;
    public GameObject Bomb;
    public float seconds;



    HUD hud;

    bool flag = false;

    void Start()
    {
        hud = FindObjectOfType<HUD>();
        enemies = FindObjectsOfType<RealEnemyBase>();
        float newMultiplier = 1f;
        switch (hud.CurrentLvl)
        {
            case 1:
                newMultiplier = PlayerPrefs.GetFloat("Difficulty1");
               
                break;
            case 2:
                newMultiplier = PlayerPrefs.GetFloat("Difficulty2");
               
                break;
            case 3:
                newMultiplier = PlayerPrefs.GetFloat("Difficulty3");
                
                break;
            case 4:
                newMultiplier = PlayerPrefs.GetFloat("Difficulty4");
             
                break;
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].health *= newMultiplier;
            enemies[i].GetComponentInChildren<AttackHit>().hitPower *= newMultiplier;
            enemyList.Add(enemies[i]);
            
        }
       // BombPrefab.GetComponentInChildren<AttackHit>().hitPower *= newMultiplier;


    }

    // Update is called once per frame
    void Update()
    {
        
        for(int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            }
        }
        count = enemyList.Count;
        if (enemyList.Count <= 0)
        {
            hud.condition = 1;
            Debug.Log("Win");
        }
        if (hud.CurrentLvl != 3)
        {
            seconds -= Time.deltaTime;
            if (seconds < 0 && !flag)
            {
                flag = true;
                StartCoroutine(BombSpawn());
            }
        }
       
    }

    IEnumerator BombSpawn()
    {
        while (true)
        {
            Instantiate(Bomb, new Vector3(BombSpawnPoint.transform.position.x, BombSpawnPoint.transform.position.y, BombSpawnPoint.transform.position.z),Quaternion.identity,null);
            yield return new WaitForSecondsRealtime(3f);
        }
    }


}

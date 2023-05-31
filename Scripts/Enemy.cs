using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{   
    public Animator animator;

    public GameObject winScreen;

    public float MaxHP = 500f;
    public float currentHP;


    void Start()
    {
        currentHP = MaxHP;
    }

    public void TakeDamage(int damage)
    {
       currentHP -= damage;
       animator.SetTrigger("Hurt");

       if(currentHP <= 0)
       {
            if(!winScreen.activeSelf)
            {
               winScreen.SetActive(true);
           }
            Die();

       }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;

        this.enabled = false;
        
    }

}

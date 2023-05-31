using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Fight : MonoBehaviour
{
    public Animator animator;

    public Transform Attack_Mark;

    public LayerMask enemyLayers;

    public float attackRange = 0.5f;

    public int attackDamage = 40;

    public float attackRate = 2f;

    float nextAttackTime = 0f;


    void Update()
    {   
       if(Time.time >= nextAttackTime)
       {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            } 
       }
       
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Attack_Mark.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
           enemy.GetComponent<Enemy>().TakeDamage(attackDamage);

           Debug.Log("Вы отоковали Босса!!!");
        }

    }

    void OnDrawGizmosSelected()
    {   
        if(Attack_Mark == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(Attack_Mark.position, attackRange);
    }
}

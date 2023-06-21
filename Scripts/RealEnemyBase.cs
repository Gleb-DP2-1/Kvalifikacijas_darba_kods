using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The core functionality of both the EnemyFlyer and the EnemyWalker*/

[RequireComponent(typeof(RecoveryCounter))]

public class RealEnemyBase : MonoBehaviour
{
    [Header("Reference")]
    [System.NonSerialized] public AudioSource audioSource;
    public Animator animator;
   
    //[SerializeField] Instantiator instantiator;
    [System.NonSerialized] public RecoveryCounter recoveryCounter;

    [Header("Properties")]
   
    [SerializeField] public float health;
    public AudioClip hitSound;
    public bool isBomb;
    public bool isExpoder;
    [SerializeField] bool requirePoundAttack; //Requires the player to use the down attack to hurt
    [SerializeField] ParticleSystem HurtEffect;
    [SerializeField] GameObject deathExplosion;

    void Start()
    {
        recoveryCounter = GetComponent<RecoveryCounter>();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
      
        if (health <= 0)
        {
            Die();
        }
    }

    public void GetHurt(int launchDirection, int hitPower)
    {
        //Hit the enemy, causing a damage effect, and decreasing health. Allows for requiring a downward pound attack
        if ((GetComponent<Walker>() != null || GetComponent<Flyer>() != null) && !recoveryCounter.recovering)
        {
            if (!requirePoundAttack || (requirePoundAttack && RealMovement.Instance.pounding))
            {
                Instantiate(HurtEffect, transform);
                health -= hitPower;
                animator.SetTrigger("hurt");

                //audioSource.pitch = (1);
                //audioSource.PlayOneShot(hitSound);

                //Ensure the enemy and also the player cannot engage in hitting each other for the max recoveryTime for each
                recoveryCounter.counter = 0;
                RealMovement.Instance.recoveryCounter.counter = 0;

                if (RealMovement.Instance.pounding)
                {
                    RealMovement.Instance.PoundEffect();
                }


                //The Walker being launched after getting hit is a little different than the Flyer getting launched by a hit.
                if (GetComponent<Walker>() != null)
                {
                    Walker walker = GetComponent<Walker>();
                    walker.launch = launchDirection * walker.hurtLaunchPower / 5;
                    walker.velocity.y = walker.hurtLaunchPower;
                    walker.directionSmooth = launchDirection;
                    walker.direction = walker.directionSmooth;
                }

                if (GetComponent<Flyer>() != null)
                {
                    Flyer flyer = GetComponent<Flyer>();
                    flyer.speedEased.x = launchDirection * 5;
                    flyer.speedEased.y = 4;
                    flyer.speed.x = flyer.speedEased.x;
                    flyer.speed.y = flyer.speedEased.y;
                }

                RealMovement.Instance.FreezeFrameEffect();
            }
        }
    }

    public void Die()
    {
        if (RealMovement.Instance.pounding)
        {
            RealMovement.Instance.PoundEffect();
        }

        health = 0;
        GameObject deathEffect = Instantiate(deathExplosion, transform);
        deathEffect.transform.parent = transform.parent;
        

        //instantiator.InstantiateObjects();
        // Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
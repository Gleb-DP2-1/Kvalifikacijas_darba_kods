using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

	public  float HP = 100f;
	public float CurHP;

	public Animator animator;

	public GameObject deathScreen;

	public GameObject deathEffect;

	void Start()
	{
		CurHP = HP;
	}

	public void TakeDamage(int damage)
	{
		
		CurHP -= damage;

		StartCoroutine(DamageAnimation());


		if (CurHP <= 0)
		{
			if(!deathScreen.activeSelf)
            {
               deathScreen.SetActive(true);
            }
			Die();
		}
	}

	void Die()
	{
		Debug.Log("You died!");

        animator.SetBool("IsDead", true);

        GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;

	
	}
	IEnumerator DamageAnimation()
	{
		SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

		for (int i = 0; i < 3; i++)
		{
			foreach (SpriteRenderer sr in srs)
			{
				Color c = sr.color;
				c.a = 0;
				sr.color = c;
			}

			yield return new WaitForSeconds(.1f);

			foreach (SpriteRenderer sr in srs)
			{
				Color c = sr.color;
				c.a = 1;
				sr.color = c;
			}

			yield return new WaitForSeconds(.1f);
		}
	}


}


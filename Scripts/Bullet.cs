using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    private int fire_direction;
    private float start_pos;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        if (RealMovement.Instance.transform.position.x > this.transform.position.x)
        {
            fire_direction = -1;
            this.start_pos = this.transform.position.x;
        }
        else if (RealMovement.Instance.transform.position.x < this.transform.position.x)
        {
            fire_direction = 1;
            this.start_pos = this.transform.position.x;

        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10)
        {
            Destroy(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        RealEnemyBase enemy = col.GetComponent<RealEnemyBase>();
        if (enemy != null)
        {
            enemy.GetHurt(fire_direction,Mathf.RoundToInt(RealMovement.Instance.maxMana*0.2f));
            Destroy(gameObject);
        }

    }


}

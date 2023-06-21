using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(RecoveryCounter))]

public class RealMovement : PhysicsObject
{
    [Header("Reference")]
    public AudioSource audioSource;
    [SerializeField] private Animator animator;
    [System.NonSerialized]public bool Attacking = false;
    public GameObject attackHit;
    private CapsuleCollider2D capsuleCollider;

    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private AudioSource flameParticlesAudioSource;
    [SerializeField] private GameObject graphic;
    [SerializeField] private Component[] graphicSprites;
    [SerializeField] private GameObject jumpParticle;
    [SerializeField] private GameObject pauseMenu;
    public RecoveryCounter recoveryCounter;

    [SerializeField] Collider2D AttackCollider;


    private static RealMovement instance;
    public static RealMovement Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<RealMovement>();
            return instance;
        }
    }

    [Header("Properties")]
    [SerializeField] private string[] cheatItems;
    public bool dead = false;
    public bool frozen = false;
    private float fallForgivenessCounter; //Counts how long the player has fallen off a ledge
    [SerializeField] private float fallForgiveness = .2f; //How long the player can fall from a ledge and still jump
    [System.NonSerialized] public string groundType = "grass";
    [System.NonSerialized] public RaycastHit2D ground;
    [SerializeField] Vector2 hurtLaunchPower; //How much force should be applied to the player when getting hurt?
    private float launch; //The float added to x and y moveSpeed. This is set with hurtLaunchPower, and is always brought back to zero
    [SerializeField] private float launchRecovery; //How slow should recovering from the launch be? (Higher the number, the longer the launch will last)
    public float maxSpeed = 7; //Max move speed
    public float sprintSpeed = 12;
    public float baseSpeed;
    public float jumpPower = 17;
    private bool jumping;
    private Vector3 origLocalScale;
    [System.NonSerialized] public bool pounded;
    [System.NonSerialized] public bool pounding;
    [System.NonSerialized] public bool shooting = false;
    public bool hasGun = false;
    public SpriteRenderer spriteRenderer;
    //public Sprite MeleeSprite;
    //public Sprite RangedSprite;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Rigidbody2D rb;
    public bool GoingRight = true;
    public int dmg;
    public bool isDashing = false;
    public float dashDistance = 15f;
    public TrailRenderer trail;


    public Camera camera;



    public bool sprinting = false;



    public int healthPotionCount = 0;

    public int waterPotionCount = 0;

    public float direction;

    HUD hud;
    public enum characters { }



    [Header("Inventory")]
    public float ammo;
    public int coins;
    public int health;

    public int maxHealth;
    public int baseHealth;

    public float maxStamina;
    public float stamina;
    public float baseStamina;

    public int baseDamage = 10;

    [Header("Sounds")]
    public AudioClip deathSound;
    public AudioClip equipSound;
    public AudioClip grassSound;
    public AudioClip hurtSound;
    public AudioClip[] hurtSounds;

    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip poundSound;
    public AudioClip punchSound;
    public AudioClip[] poundActivationSounds;

    public AudioClip stepSound;
    [System.NonSerialized] public int whichHurtSound;


    [Header("Mana")]
    public float baseMana;
    public float mana;
    public float maxMana;
    public GameObject ballPrefab;

    public ParticleSystem slashAttack;
    public ParticleSystem HurtParticle;
    public GameObject deadParticles;


    public bool GodMode;

    [Header("Gear Bonus")]
    public float healthBonus;
    public float manaBonus;
    public float attackBonus;
    public float speedBonus;
    public float staminaBonus;

    [SerializeField] bool Disfunctional;


    void Start()
    {
        baseStamina = 200f;
        baseSpeed = 12f;
        baseMana = 50f;
        baseHealth = 100;
        GodMode = false;
        // Cursor.visible = false;
        maxMana = mana;
              
        health = maxHealth;

        //dmg+= PlayerPrefs.GetInt("Attack");

        origLocalScale = transform.localScale;
        recoveryCounter = GetComponent<RecoveryCounter>();

       

        SetGroundType();
    }

    private void Update()
    {if(!Disfunctional)
        {
            if (GodMode)
            {
                health = maxHealth;
                stamina = maxStamina;
                mana = maxMana;
            }
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            if (mana > maxMana)
            {
                mana = maxMana;
            }
            //lightt = light.transform.position;
            //lightt.z = -10f;
            // light.transform.position = lightt;
            ComputeVelocity();

            if (stamina < maxStamina && !sprinting && stamina < maxStamina)
            {
                stamina += 5f * Time.deltaTime;
            }
            if (stamina > 0 && sprinting)
            {

                stamina -= 7 * Time.deltaTime;


            }
        }


    }

    protected void ComputeVelocity()
    {
        if (velocity.y == 0)
        {
            jumping = false;
            grounded = true;
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
           
            grounded = false;

        }
        //Player movement & attack
        Vector2 move = Vector2.zero;
        ground = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up);

        //Lerp launch back to zero at all times
        launch += (0 - launch) * Time.deltaTime * launchRecovery;

        /*if (Input.GetButtonDown("Cancel"))
        {
            pauseMenu.SetActive(true);
        }*/

        //Movement, jumping, and attacking!
        if (!frozen)
        {

            move.x = Input.GetAxis("Horizontal") + launch;

            if (Input.GetButtonDown("Jump") && !jumping && stamina >= 10)
            {
                if (!jumping)
                {
                    if (stamina >= 10)
                    {

                        stamina -= 10;

                    }


                    Jump(1f);
                    GameObject jumpEffect = Instantiate(jumpParticle, transform);
                    jumpParticle.transform.parent = transform.parent;
                }



            }



           







            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (stamina >= 10)
                {
                    maxSpeed = sprintSpeed;
                    sprinting = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                sprinting = false;
                maxSpeed = 7;
            }

            //Flip the graphic's localScale
            if (move.x > 0.01f && !GoingRight)
            {
                //old flipping system
                //graphic.transform.localScale = new Vector3(origLocalScale.x, transform.localScale.y, transform.localScale.z);
                graphic.transform.Rotate(0f, 180f, 0f);
                GoingRight = true;

            }
            else if (move.x < -0.01f && GoingRight)
            {
                //graphic.transform.localScale = new Vector3(-origLocalScale.x, transform.localScale.y, transform.localScale.z);
                graphic.transform.Rotate(0f, 180f, 0f);
                GoingRight = false;
            }

            //Punch
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {

                if (stamina >= 5 && !Attacking)
                {
                    stamina -= 5;
                    animator.SetTrigger("Attack");
                    slashAttack.Play();

                }







            }




            //Allow the player to jump even if they have just fallen off an edge ("fall forgiveness")
            if (!grounded)
            {
                if (fallForgivenessCounter < fallForgiveness && !jumping)
                {
                    fallForgivenessCounter += Time.deltaTime;
                }
                else
                {
                    animator.SetBool("grounded", false);
                }
            }
            else
            {
                fallForgivenessCounter = 0;
                animator.SetBool("grounded", true);
            }

            //Set each animator float, bool, and trigger to it knows which animation to fire
            animator.SetFloat("Speed", Mathf.Abs(velocity.x) / maxSpeed);
            //animator.SetFloat("velocityY", velocity.y);
            //animator.SetInteger("attackDirectionY", (int)Input.GetAxis("VerticalDirection"));
            //animator.SetInteger("moveDirection", (int)Input.GetAxis("HorizontalDirection"));

            targetVelocity = move * maxSpeed;

            if (mana >= 3 && !isDashing && Input.GetKeyDown(KeyCode.Z))
            {


                if (GoingRight)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
                //move.x = direction;
                StartCoroutine(Dash(direction));




            }
            if (mana >= 5&&Input.GetKeyDown(KeyCode.C))
            {
                ShootFireBall();
                mana -= 5;
            }
        }
        else
        {
            //If the player is set to frozen, his launch should be zeroed out!
            launch = 0;
        }
    }

    public void ShootFireBall()
    {
        Instantiate(ballPrefab, transform.position, transform.rotation);
    }

    public void SetGroundType()
    {
        //If we want to add variable ground types with different sounds, it can be done here
        switch (groundType)
        {
            case "Grass":
                stepSound = grassSound;
                break;
        }
    }

    public void Freeze(bool freeze)
    {
        //Set all animator params to ensure the player stops running, jumping, etc and simply stands
        if (freeze)
        {
            animator.SetInteger("moveDirection", 0);
            animator.SetBool("grounded", true);
            animator.SetFloat("velocityX", 0f);
            animator.SetFloat("velocityY", 0f);
            GetComponent<PhysicsObject>().targetVelocity = Vector2.zero;
        }

        frozen = freeze;
        shooting = false;
        launch = 0;
    }


    public void GetHurt(int hurtDirection, float hitPower, bool isBomb)
    {
        //If the player is not frozen (ie talking, spawning, etc), recovering, and pounding, get hurt!
        if (!frozen && !recoveryCounter.recovering && !pounding)
        {
            //HurtEffect();
            HurtParticle.Play();
            //animator.SetTrigger("hurt");
            velocity.y = hurtLaunchPower.y;
            launch = hurtDirection * (hurtLaunchPower.x);
            recoveryCounter.counter = 0;
          
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                health -=Mathf.RoundToInt( hitPower);
            }

         
        }
    }

   

    public IEnumerator FreezeFrameEffect(float length = .007f)
    {
        Time.timeScale = .1f;
        yield return new WaitForSeconds(length);
        Time.timeScale = 1f;
    }


    public IEnumerator Die()
    {
        if (!frozen)
        {
            animator.SetBool("Death", true);
            dead = true;
            Instantiate(deadParticles, transform);
            //deathParticles.Emit(10);
            //GameManager.Instance.audioSource.PlayOneShot(deathSound);
           
            //Time.timeScale = .6f;
            yield return null;
            //GameManager.Instance.hud.animator.SetTrigger("coverScreen");
            //GameManager.Instance.hud.loadSceneName = SceneManager.GetActiveScene().name;
            //Time.timeScale = 1f;
        }
    }

    public void ResetLevel()
    {
        Freeze(true);
        dead = false;
        health = maxHealth;
    }

    

    public void Jump(float jumpMultiplier)
    {
        if (velocity.y != jumpPower)
        {
            velocity.y = jumpPower * jumpMultiplier; //The jumpMultiplier allows us to use the Jump function to also launch the player from bounce platforms
            //PlayJumpSound();
            //PlayStepSound();
            JumpEffect();
            jumping = true;
        }
    }

    public IEnumerator Dash(float direction)
    {
        isDashing = true;
        trail.enabled = true;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        
            mana -= 3;
        

        yield return new WaitForSeconds(1f);
       trail.enabled = false;
        yield return new WaitForSeconds(0.5f);
        isDashing = false;

        rb.gravityScale = gravity;


    }



   

    public void JumpEffect()
    {
        //jumpParticles.Emit(1);
        audioSource.pitch = (Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(landSound);
    }

    public void LandEffect()
    {
        if (jumping)
        {
           // jumpParticles.Emit(1);
            audioSource.pitch = (Random.Range(0.6f, 1f));
            audioSource.PlayOneShot(landSound);
            jumping = false;
        }
    }

    public void PunchEffect()
    {
        GameManager.Instance.audioSource.PlayOneShot(punchSound);

    }

    public void ActivatePound()
    {
        //A series of events needs to occur when the player activates the pound ability
        if (!pounding)
        {
            animator.SetBool("pounded", false);

            if (velocity.y <= 0)
            {
                velocity = new Vector3(velocity.x, hurtLaunchPower.y / 2, 0.0f);
            }

            GameManager.Instance.audioSource.PlayOneShot(poundActivationSounds[Random.Range(0, poundActivationSounds.Length)]);
            pounding = true;
            FreezeFrameEffect(.3f);
        }
    }
    public void PoundEffect()
    {
        //As long as the player as activated the pound in ActivatePound, the following will occur when hitting the ground.
        if (pounding)
        {
            animator.ResetTrigger("attack");
            velocity.y = jumpPower / 1.4f;
            animator.SetBool("pounded", true);
            GameManager.Instance.audioSource.PlayOneShot(poundSound);

            pounding = false;
            recoveryCounter.counter = 0;
            animator.SetBool("pounded", true);
        }
    }

    public void FlashEffect()
    {
        //Flash the player quickly
        animator.SetTrigger("flash");
    }

    public void EnableCollider() {
        AttackCollider.enabled = true;
            
            }
    public void DisableCollider() {
        AttackCollider.enabled = false;
    
    }
}

  


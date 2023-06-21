using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : PhysicsObject
{
    [Header("Reference")]
    public RealEnemyBase enemyBase;
    [SerializeField] private GameObject graphic;

    [Header("Properties")]
    [SerializeField] private LayerMask layerMask; //What can the Walker actually touch?
    [SerializeField] enum EnemyType { Bug, Zombie }; //Bugs will simply patrol. Zombie's will immediately start chasing you forever until you defeat them.
    [SerializeField] EnemyType enemyType;
    [SerializeField] bool isBoss;

    public float attentionRange;
    public float changeDirectionEase = 1; //How slowly should we change directions? A higher number is slower!
    [System.NonSerialized] public float direction = 1;
    private Vector2 distanceFromPlayer; //How far is this enemy from the player?
    [System.NonSerialized] public float directionSmooth = 1; //The float value that lerps to the direction integer.
    [SerializeField] private bool followPlayer;
    [SerializeField] private bool flipWhenTurning = false; //Should the graphic flip along localScale.x?
    private RaycastHit2D ground;
    public float hurtLaunchPower = 10; //How much force should be applied to the player when getting hurt?
    [SerializeField] private bool jumping;
    public float jumpPower = 7;
    [System.NonSerialized] public bool jump = false;
    [System.NonSerialized] public float launch = 1; //The float added to x and y moveSpeed. This is set with hurtLaunchPower, and is always brought back to zero
    public float maxSpeed = 7;
    [SerializeField] private float maxSpeedDeviation; //How much should we randomly deviate from maxSpeed? Ensures enemies don't move at exact same speed, thus syncing up.
    [SerializeField] private bool neverStopFollowing = false; //Once the player is seen by an enemy, it will forever follow the player.
    private Vector3 origScale;
    [SerializeField] private Vector2 rayCastSize = new Vector2(1.5f, 1); //The raycast size: (Width, height)
    private Vector2 rayCastSizeOrig;
    [SerializeField] private Vector2 rayCastOffset;
    private RaycastHit2D rightWall;
    private RaycastHit2D leftWall;
    private RaycastHit2D rightLedge;
    private RaycastHit2D leftLedge;

    private float sitStillMultiplier = 1; //If 1, the enemy will move normally. But, if it is set to 0, the enemy will stop completely. 
    [SerializeField] private bool sitStillWhenNotFollowing = false; //Controls the sitStillMultiplier

    [Header("Sounds")]
    public AudioClip jumpSound;
    public AudioClip stepSound;

    [Header("Boss")]
    public float timer;
    public int phaseCount;
    public Transform[] BugsSpawn;
    public Transform[] ZombieSpawn;
    public Transform[] BombSpawn;
    public GameObject SpawnEffect;
    public GameObject Zombie;
    public GameObject Bug;

    public GameObject Bomb;

    public Transform[] BoxSpawn;
    public GameObject[] Boxes;
    public GameObject PoofEffect;

    void Start()
    {
        if (isBoss)
        {
            timer = 15f;
            phaseCount = 2;
        }
        enemyBase = GetComponent<RealEnemyBase>();
        origScale = transform.localScale;
        rayCastSizeOrig = rayCastSize;
        maxSpeed -= Random.Range(0, maxSpeedDeviation);
        launch = 0;
        if (enemyType == EnemyType.Zombie)
        {
            direction = 0;
            directionSmooth = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);
    }

    private void Update()
    {
        ComputeVelocity();

        timer += 1 * Time.deltaTime;
        if (timer >= 30f)
        {
            timer = 0;
            phaseCount++;
            if (phaseCount > 2)
            {
                phaseCount = 0;
            }
            switch (phaseCount)
            {
                case 0:
                    StartCoroutine(SpawnSkeletons());
                    break;
                case 1:
                    StartCoroutine(SpawnRockets());
                    break;
                case 2:
                    SpawnBoxes();
                        timer =15f;
                    break;
            }
        }
    }
    IEnumerator SpawnSkeletons()
    {
        for (int i = 0; i < BugsSpawn.Length; i++)
        {
            Instantiate(SpawnEffect, BugsSpawn[i]);
        }
        for (int i = 0; i < ZombieSpawn.Length; i++)
        {
            Instantiate(SpawnEffect, ZombieSpawn[i]);
        }
        yield return new WaitForSeconds(3f);
        float multiplier = 1f;
        /*for (int i = 0; i < BugsSpawn.Length; i++)
        {
            GameObject monster=Instantiate(Bug, BugsSpawn[i]);
            multiplier = PlayerPrefs.GetFloat("Difficulty3");
            monster.GetComponent<RealEnemyBase>().health *= multiplier;
            monster.GetComponentInChildren<AttackHit>().hitPower *= multiplier;
        }*/
        for (int i = 0; i < ZombieSpawn.Length; i++)
        {
            GameObject monster = Instantiate(Zombie, ZombieSpawn[i]);
            multiplier = PlayerPrefs.GetFloat("Difficulty3");
            monster.GetComponent<RealEnemyBase>().health *= multiplier;
            monster.GetComponentInChildren<AttackHit>().hitPower *= multiplier;
        }

    }

    IEnumerator SpawnRockets()
    {
        while (phaseCount == 1)
        {
          for(int i = 0; i < BombSpawn.Length; i++)
            {
                Instantiate(Bomb, new Vector3(BombSpawn[i].transform.position.x, BombSpawn[i].transform.position.y, BombSpawn[i].transform.position.z), Quaternion.identity, null);
            }

            yield return new WaitForSeconds(3f);

        }
    }

    void SpawnBoxes()
    {
        for (int i = 0; i < BoxSpawn.Length; i++)
        {
            Instantiate(PoofEffect, BoxSpawn[i]);
            Instantiate(Boxes[i], BoxSpawn[i]);
        }
    }
    protected void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (!RealMovement.Instance.frozen)
        {
            distanceFromPlayer = new Vector2(RealMovement.Instance.gameObject.transform.position.x - transform.position.x, RealMovement.Instance.gameObject.transform.position.y - transform.position.y);
            directionSmooth += ((direction * sitStillMultiplier) - directionSmooth) * Time.deltaTime * changeDirectionEase;
            move.x = (1 * directionSmooth) + launch;
            launch += (0 - launch) * Time.deltaTime;

            if (move.x < 0)
            {
                transform.localScale = new Vector3(-origScale.x, origScale.y, origScale.z);
            }
            else
            {
                transform.localScale = new Vector3(origScale.x, origScale.y, origScale.z);
            }

            if (!enemyBase.recoveryCounter.recovering)
            {
                //Flip the graphic depending on the speed
                if (move.x > 0.01f)
                {
                    if (graphic.transform.localScale.x == -1)
                    {
                        graphic.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    }
                }
                else if (move.x < -0.01f)
                {
                    if (graphic.transform.localScale.x == 1)
                    {
                        graphic.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    }
                }

                //Check floor type
                ground = Physics2D.Raycast(transform.position, -Vector2.up);
                Debug.DrawRay(transform.position, -Vector2.up, Color.green);

                //Check if player is within range to follow
                if (enemyType == EnemyType.Zombie)
                {
                    if ((Mathf.Abs(distanceFromPlayer.x) < attentionRange) && (Mathf.Abs(distanceFromPlayer.y) < attentionRange))
                    {
                        followPlayer = true;
                        sitStillMultiplier = 1;

                        if (neverStopFollowing)
                        {
                            attentionRange = 10000000000;
                        }
                    }
                    else
                    {
                        if (sitStillWhenNotFollowing)
                        {
                            sitStillMultiplier = 0;
                        }
                        else
                        {
                            sitStillMultiplier = 1;
                        }
                    }
                }

                if (followPlayer)
                {
                    rayCastSize.y = 200;
                    if (distanceFromPlayer.x < 0)
                    {
                        direction = -1;
                    }
                    else
                    {
                        direction = 1;
                    }
                }
                else
                {
                    rayCastSize.y = rayCastSizeOrig.y;
                }

                //Check for walls
                rightWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.right, rayCastSize.x, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.right * rayCastSize.x, Color.yellow);

                if (rightWall.collider != null)
                {
                    if (!followPlayer)
                    {
                        direction = -1;
                    }
                    else if (direction == 1)
                    {
                        Jump();
                    }

                }

                leftWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.left, rayCastSize.x, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.left * rayCastSize.x, Color.blue);

                if (leftWall.collider != null)
                {
                    if (!followPlayer)
                    {
                        direction = 1;
                    }
                    else if (direction == -1)
                    {
                        Jump();
                    }
                }

                //Check for ledges. Walker's height check is much higher! They will fall pretty far, but will not fall to death. 
                rightLedge = Physics2D.Raycast(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down, rayCastSize.y, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down * rayCastSize.y, Color.blue);
                if ((rightLedge.collider == null || rightLedge.collider.gameObject.layer == 14) && direction == 1)
                {
                    direction = -1;
                }

                leftLedge = Physics2D.Raycast(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down, rayCastSize.y, layerMask);
                Debug.DrawRay(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down * rayCastSize.y, Color.blue);
                if ((leftLedge.collider == null || leftLedge.collider.gameObject.layer == 14) && direction == -1)
                {
                    direction = 1;
                }
            }
        }

        enemyBase.animator.SetBool("grounded", grounded);
        enemyBase.animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }

    public void Jump()
    {
        if (grounded)
        {
            velocity.y = jumpPower;
            PlayJumpSound();
            PlayStepSound();
        }
    }

    public void PlayStepSound()
    {
        enemyBase.audioSource.pitch = (Random.Range(0.6f, 1f));
        enemyBase.audioSource.PlayOneShot(stepSound);
    }

    public void PlayJumpSound()
    {
        enemyBase.audioSource.pitch = (Random.Range(0.6f, 1f));
        enemyBase.audioSource.PlayOneShot(jumpSound);
    }

}
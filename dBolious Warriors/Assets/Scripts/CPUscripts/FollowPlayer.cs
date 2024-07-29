using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowPlayer : MonoBehaviour
{
    public Transform enemyPlayer;
    public Transform leftLedge;
    public Transform rightLedge;
    public Transform centerStage;
    public float attackDistance;
    public float shootDistance;

    public Animator animate;
    public GameObject player;
    private Rigidbody2D Controller;
    public LayerMask playerLayer;
    private Vector3 playerVelocity;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public CPUhealth health;

    public AudioClip audioClip;
    private AudioSource audioSource;

    public float jumpHeight = 1.0f;
    private float groundCheckSize = 0.2f;
    private bool grounded = false;

    public float runSpeed = 40f;
    private float horizontalMovement;
    private bool isFacingRight;

    private float nextAttackTime = 0f;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootRate = 5f;

    public Transform fAttackPoint;
    public float fAttackRange = 0.5f;
    public int fAttackDamage = 14;
    public float fAttackRate = 1f;

    void Start()
    {
        Controller = GetComponent<Rigidbody2D>();

        if (gameObject.GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        audioSource.clip = audioClip;

    }

    void FixedUpdate()
    {
        

        if (health == null || Controller == null)
        {
            Debug.LogError("Health or Controller is not assigned.");
            return;
        }

        if (health.canMove == false)
        {
            return;
        }

        IsGrounded();

        float distanceEnemy = Vector2.Distance(gameObject.transform.position, enemyPlayer.transform.position);

        //CPU Artificial intelligence
        if (IsWithinRadius(leftLedge, 2)) //keeps CPU from following player off of the ledge
        {
            Debug.Log("leftLedge");
            transform.position = Vector2.MoveTowards(transform.position, centerStage.position, runSpeed * Time.deltaTime);
        }
        else if (IsWithinRadius(rightLedge, 2))
        {
            Debug.Log("RightLedge");
            transform.position = Vector2.MoveTowards(transform.position, centerStage.position, runSpeed * Time.deltaTime);
        }
        else if (distanceEnemy > shootDistance) //Shoots player when at distance
        {
            Shoot();
            transform.position = Vector2.MoveTowards(transform.position, enemyPlayer.position, runSpeed * Time.deltaTime);

        } else if(distanceEnemy < attackDistance) //attacks player when in close range
        {
            fAttack();
            transform.position = Vector2.MoveTowards(transform.position, centerStage.position, runSpeed * Time.deltaTime);
        } else if(shootDistance > distanceEnemy && distanceEnemy > attackDistance) //creates variety in CPU gameplan
        {
            int randomInt = Random.Range(0, 10);
            if (randomInt == 1)
            {
                Jump();
                transform.position = Vector2.MoveTowards(transform.position, centerStage.position, runSpeed * Time.deltaTime);
            }
        }
        
        //Flips CPU when turning
        shouldFlip();

    }

    public void IsGrounded()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckSize, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight; //gives opposite value
        transform.Rotate(0f, 180f, 0f);
    }

    void Shoot()
    {
        if (health.canMove == false)
        {
            return;
        }

        if (Time.time > nextAttackTime) //creates a limit to amount of shots per second
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextAttackTime = 0;

            nextAttackTime = Time.time + 1f / shootRate;
        }
    }

    void fAttack()
    {
        if (health.canMove == false)
        {
            return;
        }

        if (Time.time > nextAttackTime) //creates a limit to amount of attacks per second
        {
            animate.SetTrigger("fAttack");
            PlayAudio();

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(fAttackPoint.position, fAttackRange, playerLayer);

            foreach (Collider2D enemy in hitPlayers)
            {
                Debug.Log(enemy.name);
                enemy.GetComponent<playerHealth>().takeDamage(fAttackDamage, player);
            }

            nextAttackTime = Time.time + 1f / fAttackRate;

        }
    }

    void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.Play();
        }
    }

    bool IsWithinRadius(Transform target, float radius)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance <= radius;
    }

    void shouldFlip()
    {
        if (!isFacingRight && transform.position.x > enemyPlayer.position.x)
        {
            Flip();
        }
        else if (isFacingRight && transform.position.x < enemyPlayer.position.x)
        {
            Flip();
        }
    }
    public void Jump()
    {
        if (health.canMove == false)
        {
            return;
        }

        if (grounded)
        {
            Controller.velocity = new Vector2(Controller.velocity.x, jumpHeight); 
        }

    }
}

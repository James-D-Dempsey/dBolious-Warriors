using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Very similar to CPUhealth.cs, Main differences are different variable types that are player based rather than cpu based
//Additionally, rather than having if statements to simulate human gameplay, this game makes use of Unitys Input System 

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    public Animator animate;
    public GameObject player;
    private Rigidbody2D Controller;
    public LayerMask playerLayer;
    private Vector3 playerVelocity;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private PlayerInput PlayerInput;
    public playerHealth health;

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
    
    public Transform fAttackPoint;
    public float fAttackRange = 0.5f;
    public int fAttackDamage = 14;
    public float fAttackRate = 1f;

    private void Awake()
    {

    }

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

        Controller.velocity = new Vector2(horizontalMovement * runSpeed, Controller.velocity.y); 

        if (!isFacingRight && horizontalMovement < 0f)
        {
            Flip(); 
        }
        else if (isFacingRight && horizontalMovement > 0f)
        {
            Flip();
        }

    }
    void Shoot()
    {
        if (health.canMove == false)
        {
            return;
        }

        if (Time.time > nextAttackTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextAttackTime = 0;
        }
    }

    public void onShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }

    void fAttack()
    {
        if (health.canMove == false)
        {
            return;
        }

        if (Time.time > nextAttackTime)
        {
            animate.SetTrigger("fAttack");
            PlayAudio();

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(fAttackPoint.position, fAttackRange, playerLayer);

            foreach (Collider2D enemy in hitPlayers)
            {
                Debug.Log(enemy.name);
                enemy.GetComponent<CPUhealth>().takeDamage(fAttackDamage, player);
            }

            nextAttackTime = Time.time + 1f / fAttackRate;
            
        }
    }

    public void onfAttack(InputAction.CallbackContext context)
    {
        fAttack();
    }

    public void Walk(InputAction.CallbackContext context)
    {
        if (health.canMove == false)
        {
            return;
        }

        horizontalMovement = context.ReadValue<Vector2>().x; 

        if (horizontalMovement > 0 && horizontalMovement < 1)
        {
            horizontalMovement = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -1)
        {
            horizontalMovement = -1; 
        }
    }

    public void Jump(InputAction.CallbackContext context)
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
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.Play();
        }
    }
}

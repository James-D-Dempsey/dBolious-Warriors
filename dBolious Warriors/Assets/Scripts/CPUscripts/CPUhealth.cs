using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPUhealth : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public Vector3 spawnPoint;
    public CPUinfo CPUinfo;
    public GameObject gameOverMenu;

    public GameObject player;
    public float hitPercent = 0;
    public bool canMove = true;
    public UnityEvent onBegin, onDone;

    void Start()
    {
        Time.timeScale = 1; //Ensures scene is not paused when a new game starts

        spawnPoint = new Vector3(5, 3, 0); //Hardcoded spawnpoints, will make a better implementation

        if (CPUinfo == null)
        {
            CPUinfo = GetComponent<CPUinfo>(); 

            if (CPUinfo == null)
            {
                Debug.LogError("CPUinfo is not assigned");
            }
        }
    }

    public void takeDamage(float Damage, GameObject sender)
    {
        StopAllCoroutines();
        onBegin?.Invoke();  //Leaves options open for creative ideas
        hitPercent += Damage;

        CPUinfo.i_hitPercent = hitPercent;

         Vector2 direction = (transform.position - sender.transform.position).normalized;

        float angle = 60f * Mathf.Deg2Rad; //helps angle knockback to give knockback a more platform fighter feel
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        Vector2 rotatedDirection = new Vector2(
            direction.x * cos - direction.y * sin,
            Mathf.Abs(direction.x * sin + direction.y * cos)
        );

        rb.AddForce(rotatedDirection * Damage * hitPercent / 50f, ForceMode2D.Impulse); //applys knockback
        StartCoroutine(hitStun(Damage)); //causes player to be unable to move until unstunned
    }

    public void Die()
    {

        if (CPUinfo != null)
        {
            CPUinfo.i_stockCount--;
            Debug.Log(CPUinfo.i_stockCount);
            CPUinfo.i_hitPercent = 0;
            if (CPUinfo.i_stockCount == 0)
            {
                Debug.Log("Game Over");
                Time.timeScale = 0;  //pauses game on end
                gameOverMenu.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("CPUinfo is not assigned.");
        }

        //resets positions and hitpercent on respawn
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = spawnPoint;
        hitPercent = 0;
        transform.rotation = Quaternion.identity;
    }

    public IEnumerator hitStun(float Damage)
    {
        canMove = false; //removes player ability to move
        yield return new WaitForSeconds((hitPercent / 1000f) * Damage + (1 / 5));
        rb.velocity = Vector3.zero;
        onDone?.Invoke(); //opens possibilities for creative moves
        canMove = true; //returns player ability to move
    }


}

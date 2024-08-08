using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Very similar to CPUhealth.cs, Main differences are different variable types that are player based rather than cpu

public class playerHealth : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    public Vector3 spawnPoint;
    public playerInfo playerInfo;
    public GameObject gameOverMenu;

    public GameObject player;
    public float hitPercent = 0;
    public bool canMove = true;
    public UnityEvent onBegin, onDone;

    void Start()
    {
        Time.timeScale = 1;

        spawnPoint = new Vector3(-5, 3, 0); 

        if (playerInfo == null)
        {
            playerInfo = GetComponent<playerInfo>(); 
            if (playerInfo == null)
            {
                Debug.LogError("playerInfo is not assigned and could not be found on the GameObject.");
            }
        }
    }

    public void takeDamage(float Damage, GameObject sender)
    {
        StopAllCoroutines();
        onBegin?.Invoke();
        hitPercent += Damage;

        if (playerInfo != null)
        {
            playerInfo.i_hitPercent += Damage;
        }
        else
        {
            Debug.LogError("playerInfo is not assigned.");
        }

        Vector2 direction = (transform.position - sender.transform.position).normalized; 

        float angle = 60f * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        Vector2 rotatedDirection = new Vector2(
            direction.x * cos - direction.y * sin,
            Mathf.Abs(direction.x * sin + direction.y * cos)
        );

        rb.AddForce(rotatedDirection * Damage * hitPercent / 50f, ForceMode2D.Impulse);
        StartCoroutine(hitStun(Damage));
    }

    public void Die()
    {

        if (playerInfo != null)
        {
            playerInfo.i_stockCount--;
            playerInfo.i_hitPercent = 0;
            if (playerInfo.i_stockCount == 0)
            {
                Debug.Log("Game Over");
                Time.timeScale = 0;
                gameOverMenu.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("playerInfo is not assigned.");
        }

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = spawnPoint;
        hitPercent = 0;
        transform.rotation = Quaternion.identity;
    }

    public IEnumerator hitStun(float Damage)
    {
        canMove = false;
        yield return new WaitForSeconds((hitPercent /1000f) * Damage + (1 / 5));
        rb.velocity = Vector3.zero;
        onDone?.Invoke();
        canMove = true;
    }


}

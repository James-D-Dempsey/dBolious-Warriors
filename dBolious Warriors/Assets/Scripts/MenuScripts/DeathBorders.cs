using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBorders : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        playerHealth player = collision.GetComponent<playerHealth>();
        CPUhealth CPU = collision.GetComponent<CPUhealth>();

        if (player != null) //Calls playerHealth to cause stock loss
        {
            player.Die();
        }

        if (CPU != null) //Calls CPUhealth to cause stock loss
        {

            CPU.Die();
        }
    }
}

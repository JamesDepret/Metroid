using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBall : MonoBehaviour
{
    public GameObject impactEffect;
    public int damageAmount = 1;
    private bool playerHit;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damageAmount);
            playerHit = true;
        }
        if (impactEffect != null && !playerHit)
            Instantiate(impactEffect, transform.position, Quaternion.identity);


        Destroy(gameObject);
    }
}

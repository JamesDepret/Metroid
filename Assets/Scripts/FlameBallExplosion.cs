using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBallExplosion : MonoBehaviour
{

    public int damageAmount = 1;
    private bool playerHit;
    public LayerMask whatIsPlayer;
    private void Start()
    {
        if (!playerHit)
        {
            playerHit = true;
            Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(transform.position, 3f, whatIsPlayer);
            if (objectsToHit.Length > 0)
            {
                foreach (Collider2D collider in objectsToHit)
                {
                    if(collider.tag == "Player")
                    {
                        PlayerHealthController.instance.DamagePlayer(damageAmount);
                    }
                }
            }
        }
    }
}

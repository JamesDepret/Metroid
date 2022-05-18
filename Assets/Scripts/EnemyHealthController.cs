using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth = 3;
    public GameObject deathEffect;
    public void damageEnemy(int damageAmount)
    {
        totalHealth -= damageAmount;
        if(totalHealth <= 0) {
            if(deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }
            //  this is added
            EnemyPatroller patroller = gameObject.GetComponent<EnemyPatroller>();
            if (patroller != null)
            {
                patroller.DestroyPatrolPoints();
            }

            // end addition
            Destroy(gameObject);
        }
    }
}

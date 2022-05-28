using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSimpleBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletSpeed;
    public Rigidbody2D theRB;

    public Vector2 moveDir;

    public GameObject impactEffect;
    public int damageAmount = 1;

    void Update()
    {
        theRB.velocity = moveDir * bulletSpeed;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer(damageAmount);
        }


        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

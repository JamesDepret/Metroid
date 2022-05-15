using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint;

    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    public float jumpforce;

    public Rigidbody2D enemyRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;
        foreach (Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2)
        {
            MoveHorizontally();

            //ENDED AT VIDEO finishing patrol 7:00
        } else
        {
            HoldTemporaryPosition();
        }
    }

    private void MoveHorizontally()
    {
        if (transform.position.x < patrolPoints[currentPoint].position.x)
        {
            enemyRigidBody.velocity = new Vector2(moveSpeed, enemyRigidBody.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            enemyRigidBody.velocity = new Vector2(-moveSpeed, enemyRigidBody.velocity.y);
            transform.localScale = Vector3.one;
        }
    }

    private void HoldTemporaryPosition()
    {
        enemyRigidBody.velocity = new Vector2(0f, enemyRigidBody.velocity.y);

        waitCounter -= Time.deltaTime;
        if (waitCounter <= 0)
        {
            waitCounter = waitAtPoints;
            currentPoint = (currentPoint >= patrolPoints.Length ? 0 : currentPoint + 1);
        }
    }
}

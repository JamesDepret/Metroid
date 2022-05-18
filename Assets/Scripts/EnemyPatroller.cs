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

    public float waitTimerBeforeJump = 0.5f;
    private bool atCheckPoint = false;
    private float previousPosition, previousPositionCounter;
    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;
        previousPositionCounter = waitTimerBeforeJump;
        foreach (Transform pPoint in patrolPoints)
        {
            previousPosition = transform.position.x;
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2)
        {
            MoveHorizontally();
            MoveVertically();

        } else
        {
            HoldTemporaryPosition();
        }
        if(enemyRigidBody == null) Debug.Log("dead");
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

    private void MoveVertically()
    {
        // low wall stuck - jump solution
        if (!atCheckPoint && previousPosition == transform.position.x)
        {
            if (previousPositionCounter >= 0)
            {
                previousPositionCounter -= Time.deltaTime;
            }
            else
            {
                enemyRigidBody.velocity = new Vector2(enemyRigidBody.velocity.x, jumpforce);
                previousPositionCounter = waitTimerBeforeJump;
            }
        }
        else
        {
            previousPositionCounter = waitTimerBeforeJump;
        }
        previousPosition = transform.position.x;
    }

    private void HoldTemporaryPosition()
    {
        enemyRigidBody.velocity = new Vector2(0f, enemyRigidBody.velocity.y);
        atCheckPoint = true;
        waitCounter -= Time.deltaTime;
        if (waitCounter <= 0)
        {
            atCheckPoint = false;
            waitCounter = waitAtPoints;
            currentPoint = (currentPoint +1 >= patrolPoints.Length ? 0 : currentPoint + 1);
        }
    }

    public void DestroyPatrolPoints()
    {
        foreach (Transform pPoint in patrolPoints)
        {
            Destroy(pPoint.gameObject);
        }
    }
}

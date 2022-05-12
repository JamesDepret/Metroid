using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRigidBody;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    public Animator animator;

    public BulletController shotToFire;
    public Transform shotPoint;

    void Start()
    { 

    }

    void Update()
    {
        HorizontalMovement();
        JumpMovement();
        SetupAnimator();
        ShotLogic();
    }


    private void SetupAnimator()
    {
        animator.SetBool("isOnGround", isOnGround);
        animator.SetFloat("speed", Mathf.Abs( playerRigidBody.velocity.x ));
    }
    private void ShotLogic()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);

            animator.SetTrigger("shotFired");
        }
    }

    private void JumpMovement()
    {
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, (float).2, whatIsGround);

        if (isOnGround && Input.GetButtonDown("Jump"))
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
        }
    }

    private void HorizontalMovement()
    {
        playerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRigidBody.velocity.y);
        if(playerRigidBody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f);
        } else if (playerRigidBody.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }
}

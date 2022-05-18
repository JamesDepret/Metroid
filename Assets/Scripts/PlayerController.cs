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

    private bool canDoubleJump;

    public float dashSpeed, dashTime;
    private float dashCounter;

    public SpriteRenderer playerSprite, afterImage;
    public float afterImageLifeTime, timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    public float waitAfterDashing;
    private float dashRechargeCounter;

    public GameObject standing, ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnimator;

    public Transform bombPoint;
    public GameObject bomb;

    private PlayerAbilityTracker abilities;

    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>(); // Find on runtime if the current Object has an PlayerAbilityTracker component
    }

    void Update()
    {
        HorizontalMovement();
        JumpMovement();
        BallMode();
        SetupAnimator();
        ShotLogic();
    }
    private void BallMode()
    {
        if (!ball.activeSelf)
        {
            if(Input.GetAxisRaw("Vertical") < -0.9f && abilities.canBecomeBall) // press downwards
            {
                ballCounter -= Time.deltaTime;
                if(ballCounter <= 0)
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                }
            } else
            {
                ballCounter = waitToBall;
            }
        } 
        else
        {
            if (Input.GetAxisRaw("Vertical") > 0.9f) // press downwards
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }
    }

    private void SetupAnimator()
    {
        if (standing.activeSelf)
        {
            animator.SetBool("isOnGround", isOnGround);
            animator.SetFloat("speed", Mathf.Abs( playerRigidBody.velocity.x ));
        }
        if (ball.activeSelf)
        {
            ballAnimator.SetFloat("speed", Mathf.Abs(playerRigidBody.velocity.x));
        }
    }
    private void ShotLogic()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            if (standing.activeSelf)
            {
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
                animator.SetTrigger("shotFired");
            }

            if (ball.activeSelf && abilities.canDropBomb)
            {
                Instantiate(bomb, bombPoint.position, bombPoint.rotation);
                animator.SetTrigger("shotFired");
            }
        }
    }

    private void JumpMovement()
    {
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, (float).2, whatIsGround);

        if (isOnGround)
        {
            canDoubleJump = true;
        }

        if ((isOnGround || ( canDoubleJump && abilities.canDoubleJump) ) && Input.GetButtonDown("Jump"))
        {
            
            if (!isOnGround) 
            {
                canDoubleJump = false;
                animator.SetTrigger("doubleJump");
            }
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
        } 
    }

    private void HorizontalMovement()
    {
        if(dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)
        {
            dashCounter = dashTime;

            showAfterImage();
        }

        if(dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            playerRigidBody.velocity = new Vector2(dashSpeed * transform.localScale.x, playerRigidBody.velocity.y);
            afterImageCounter -= Time.deltaTime;
            if(afterImageCounter <= 0) showAfterImage();
            dashRechargeCounter = waitAfterDashing;
        } 
        else
        {
            playerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRigidBody.velocity.y);
            if (playerRigidBody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f);
            }
            else if (playerRigidBody.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }
    }

    private void showAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = playerSprite.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifeTime);

        afterImageCounter = timeBetweenAfterImages;
    }
}

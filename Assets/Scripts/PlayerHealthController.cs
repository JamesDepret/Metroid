using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    //[HideInInspector]
    public int currentHealth;
    public int maxHealth;
    public float invincibilityLength;
    private float invincibilityCounter;

    public float flashLength;
    private float flashCounter;

    public SpriteRenderer[] playerSprites;
    public int redFlashSteps = 10;
    private int redFlashCurrentStep;
    private List<float> originalRedValues = new List<float>();
    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);

        foreach (SpriteRenderer sprite in playerSprites)
        {
            originalRedValues.Add(sprite.color.r);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FlashRed();
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if(invincibilityCounter <=0)
        {
            currentHealth -= damageAmount;

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                //gameObject.SetActive(false);
                RespawnController.instance.Respawn();
            } else
            {
                invincibilityCounter = invincibilityLength;
                redFlashCurrentStep = redFlashSteps;
            }

            UIController.instance.UpdateHealth(currentHealth, maxHealth);

        }
    }
/*
    private bool fadingToRed, fadingFromRed;
    private void FlashRed(){
        if(fadingToRed) {
            foreach (SpriteRenderer sprite in playerSprites)
            {
                SetSpriteRedColor(
                    sprite,
                    Mathf.MoveTowards(sprite.color.r,1f,flashLength * Time.deltaTime)
                );
            }
            if(playerSprites[0].color.r == 1f) {
                fadingToRed = false;
                if(invincibilityCounter < 0){
                    fadingFromRed = true;
                }
            }
        } else if (fadingFromRed) {
            int index = 0;
            foreach (SpriteRenderer sprite in playerSprites)
            {
                SetSpriteRedColor(
                    sprite,
                    Mathf.MoveTowards(sprite.color.r,originalRedValues[index],flashLength * Time.deltaTime)
                );
                index++;
            }
            if(playerSprites[0].color.r == 0f) {
                fadingFromRed = false;
                if(invincibilityCounter < 0){
                    fadingToRed = true;
                }
            }
        }
    }
    */
    private void FlashRed()
    {
        int index;
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            if (redFlashCurrentStep > 0)
            {
                index = 0;
                foreach (SpriteRenderer sprite in playerSprites)
                {

                    SetSpriteRedColor(
                        sprite,
                        sprite.color.r + (1 - originalRedValues[index]) / redFlashSteps
                    );
                    redFlashCurrentStep--;
                    index++;
                }
            }
            else if (Mathf.Abs(redFlashCurrentStep) < redFlashSteps)
            {
                redFlashCurrentStep--;
            }
            else if (Mathf.Abs(redFlashCurrentStep) < redFlashSteps * 2)
            {
                index = 0;
                foreach (SpriteRenderer sprite in playerSprites)
                {

                    SetSpriteRedColor(
                        sprite,
                        sprite.color.r - (1 - originalRedValues[index]) / redFlashSteps
                    );
                    redFlashCurrentStep--;
                    index++;
                }
            }
            else
            {
                redFlashCurrentStep = redFlashSteps;
            }

            if(invincibilityCounter <= 0)
            {
                {
                    index = 0;
                    foreach (SpriteRenderer sprite in playerSprites)
                    {
                        SetSpriteRedColor(sprite, originalRedValues[index]);
                        index++;
                    }
                }
            }
        }
    }


    private void SetSpriteRedColor(SpriteRenderer sprite, float redValue)
    {
        sprite.color = new Color(
            redValue,
            sprite.color.g,
            sprite.color.b,
            sprite.color.a);
    }

    public void RespawnHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void DeathEffect()
    {

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }
    }

    public void HealPlayer(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}

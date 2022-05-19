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
    private List<float> redValues = new List<float>();

    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);

        foreach (SpriteRenderer sprite in playerSprites)
        {
            redValues.Add(sprite.color.r);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FlashRed();
    }

    private void Awake()
    {
        instance = this;
    }

    public void DamagePlayer(int damageAmount)
    {
        if(invincibilityCounter <=0)
        {
            currentHealth -= damageAmount;

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                gameObject.SetActive(false);
            } else
            {
                invincibilityCounter = invincibilityLength;
                redFlashCurrentStep = redFlashSteps;
            }

            UIController.instance.UpdateHealth(currentHealth, maxHealth);

        }
    }
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
                        sprite.color.r + (1 - redValues[index]) / redFlashSteps
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
                        sprite.color.r - (1 - redValues[index]) / redFlashSteps
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
                        SetSpriteRedColor(sprite, redValues[index]);
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
}

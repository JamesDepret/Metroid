using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    //[HideInInspector]
    public int currentHealth;
    public int maxHealth;
    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        instance = this;
    }

    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            gameObject.SetActive(false);
        }

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}

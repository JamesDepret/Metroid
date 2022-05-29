using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;
    public int CurrentHealth = 250;
    public int PhaseTwoHealthTreshHold = 200;
    public int PhaseThreeHealthTreshHold = 150;
    public MageBattle theMage;
    // Start is called before the first frame update
    private void Awake() {
        instance = this;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage){
        CurrentHealth -= damage;
        if(CurrentHealth <= 0){
            CurrentHealth = 0;
            theMage.EndBattle();
        }
    }
}

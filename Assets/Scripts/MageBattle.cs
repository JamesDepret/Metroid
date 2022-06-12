using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBattle : MonoBehaviour
{
    private CameraController camera;
    public Transform camPosition;
    public SpriteRenderer activeSprite;
    public SpriteRenderer[] sprites;
    public float camSpeed;
    public float timeBeforeCombatStart = 5f;
    public int phaseOneMoment, phaseTwoMoment, phaseThreeMoment, battleEndTime = 273;
    public float activeTime, fadeOutTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;
    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;
    private float battleCounter = 0f;
    private bool battleStarted;
    private Animator currentAnimator;
    private PlayerHealthController thePlayer;
    private bool PhaseTwoTransition;
    private bool PhaseThreeTransition;
    public MageBulletCircle bulletCircle;
    public float bulletCircleDelay;
    private bool mageCircleActive;

    public float timeBetweenShots;
    private float shotCounter;
    public GameObject bullet;
    private Transform bulletPoint;
    public Transform bulletPointPhase1;
    public Transform bulletPointPhase2;
    public Transform bulletPointPhase3;
    private int bulletCounter;
    public float bulletPulseDelay = 0.3f;

    private bool dashDropped;
    public GameObject dashAbility;
    private bool jumpDropped;
    public GameObject jumpAbility;

    public GameObject FlameSpawner;
    public float FlameSpawnerTimeToSpawn;
    private float FlameSpawnerCounter;

    private bool battleIsActive;

    void Start()
    {
        camera = FindObjectOfType<CameraController>();
        camera.enabled = false;
        activeCounter = activeTime;
        thePlayer = PlayerHealthController.instance;
        shotCounter = 2;
        bulletPoint = bulletPointPhase1;
        FlameSpawnerCounter = FlameSpawnerTimeToSpawn;
        AudioManager.instance.PlayBossMusic();
        battleIsActive = true;
    }

    void Update()
    {
        if (battleIsActive)
        {
            camera.SetupBossCamera(camPosition, camSpeed);
            if (battleStarted)
            {
                battleCounter += Time.deltaTime;
                SetBossDirection();
                bulletCircle.UpdateLocation(activeSprite.gameObject.transform);
                if (battleCounter < phaseOneMoment)
                {
                    PhaseOne();
                }
                else if (battleCounter < phaseTwoMoment)
                {
                    if (!PhaseTwoTransition)
                    {
                        TransitionPhase(2);
                        bulletPoint = bulletPointPhase2;
                    }
                    if (targetPoint == null)
                    {
                        targetPoint = activeSprite.gameObject.transform;
                    }
                    PhaseOne();
                    PhaseTwo();
                }
                else if (battleCounter < phaseThreeMoment)
                {
                    if (!PhaseThreeTransition)
                    {
                        TransitionPhase(3);
                        bulletPoint = bulletPointPhase3;
                    }
                    if (targetPoint == null)
                    {
                        targetPoint = activeSprite.gameObject.transform;
                    }
                    PhaseOne();
                    PhaseTwo();
                    PhaseThree();
                }

                if(battleCounter > battleEndTime)
                {
                    battleIsActive = false;
                    camera.EndBossBattle();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void PhaseOne(){
        if(activeCounter > 0){
            if(targetPoint == null) { 
                activeCounter -= Time.deltaTime;
                fadeCounter = fadeOutTime;
            } else
            {
                if (Vector3.Distance(activeSprite.gameObject.transform.position, targetPoint.transform.position) > 0.02f)
                {
                    activeSprite.gameObject.transform.position = Vector3.MoveTowards(activeSprite.gameObject.transform.position, targetPoint.transform.position, moveSpeed * Time.deltaTime);
                }
                else
                {
                    activeCounter = 0;
                    fadeCounter = fadeOutTime;
                }
            }
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                bulletCounter = 0;

                StartCoroutine(BulletsCoRoutine());
            }
        }
        else if (fadeCounter > 0)
        {
            currentAnimator.SetTrigger("vanish");
            fadeCounter -= Time.deltaTime;
            if(fadeCounter <= 0){
                activeSprite.gameObject.SetActive(false);
                inactiveCounter = inactiveTime;
            }
        }
        else if (inactiveCounter > 0)
        {
            inactiveCounter -= Time.deltaTime;
            if(inactiveCounter <= 0){
                activeSprite.gameObject.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                if(targetPoint != null){
                    targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    int whileBreaker = 0;
                    while ((targetPoint.transform.position == activeSprite.gameObject.transform.position && whileBreaker < 100))
                    {
                        targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                        whileBreaker++;
                    }
                }
                activeSprite.gameObject.SetActive(true);
                activeCounter = activeTime;
            }
        }
    }

    public void PhaseTwo()
    {
        if (activeCounter > 0 && !mageCircleActive) {
            bulletCircle.FireCircle();
            mageCircleActive = true; 
            StartCoroutine(ShotsCoRoutine());
        }

        if (!dashDropped )
        {
            if(BossHealthController.instance.CurrentHealth < BossHealthController.instance.PhaseTwoHealthTreshHold)
            {
                dashAbility.SetActive(true);
                dashDropped = true;
            }
        }
    }

    public void PhaseThree()
    {

        if (!jumpDropped)
        {
            if(BossHealthController.instance.CurrentHealth < BossHealthController.instance.PhaseThreeHealthTreshHold)
            {
                jumpAbility.SetActive(true);
                jumpDropped = true;
            }
        }

        if(FlameSpawnerCounter <= 0)
        {
            FlameSpawnerCounter = FlameSpawnerTimeToSpawn;
            Instantiate(FlameSpawner);
        } else
        {
            FlameSpawnerCounter -= Time.deltaTime;
        }
    }

    IEnumerator ShotsCoRoutine()
    {
        yield return new WaitForSeconds(bulletCircleDelay);
        mageCircleActive = false;
    }

    IEnumerator BulletsCoRoutine()
    {
        yield return new WaitForSeconds(bulletPulseDelay);
        Instantiate(bullet, bulletPoint.position, Quaternion.identity);
        bulletCounter++;
        if (bulletCounter < 3)
        {
            StartCoroutine(BulletsCoRoutine());
        }
    }



    private void TransitionPhase(int phaseNumber){
        if(sprites.Length >= phaseNumber && inactiveCounter > 0 ){
            int index = phaseNumber - 1;
            Transform currentPoint = activeSprite.gameObject.transform;
            activeSprite.gameObject.SetActive(false);
            activeSprite = sprites[index];
            getAnimator();
            activeSprite.transform.position = currentPoint.position;

            if (phaseNumber == 2) PhaseTwoTransition = true;
            if (phaseNumber == 3) PhaseThreeTransition = true;
        }
    }

    private void SetBossDirection(){
        if(activeSprite != null){
            if(thePlayer.transform.position.x > activeSprite.gameObject.transform.position.x){
                activeSprite.gameObject.transform.localScale = new Vector3(Mathf.Abs(activeSprite.gameObject.transform.localScale.x), 
                                                                            activeSprite.gameObject.transform.localScale.y, 
                                                                            activeSprite.gameObject.transform.localScale.z);
            } else {
                activeSprite.gameObject.transform.localScale = new Vector3(-Mathf.Abs(activeSprite.gameObject.transform.localScale.x), 
                                                                            activeSprite.gameObject.transform.localScale.y, 
                                                                            activeSprite.gameObject.transform.localScale.z);
            }
        }
    }

    public void EndBattle(){
        gameObject.SetActive(false);
    }

    private void getAnimator(){
        currentAnimator = activeSprite.gameObject.GetComponent<Animator>();
    }
    public void StartBattle(){
        StartCoroutine(StartBattletCoRoutine());
    }

    IEnumerator StartBattletCoRoutine()
    {
        yield return new WaitForSeconds(timeBeforeCombatStart);
        activeSprite.gameObject.SetActive(true);
        getAnimator();
        battleStarted = true;
        activeCounter = activeTime;
    }

}

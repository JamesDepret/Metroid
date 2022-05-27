using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBattle : MonoBehaviour
{
    private CameraController Camera;
    public Transform CamPosition;
    public SpriteRenderer activeSprite;
    public SpriteRenderer[] sprites;
    public float CamSpeed;
    public float TimeBeforeCombatStart = 5f;
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

    void Start()
    {
        Camera = FindObjectOfType<CameraController>();
        Camera.enabled = false;
        activeCounter = activeTime;
        thePlayer = PlayerHealthController.instance;
    }

    void Update()
    {
        Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, CamPosition.position, CamSpeed * Time.deltaTime);
        if(battleStarted){
            battleCounter += Time.deltaTime;
            SetBossDirection();
            if(battleCounter < phaseOneMoment){
                phaseOne();
            } else if(battleCounter < phaseTwoMoment){
                TransitionPhase(2);
                if(targetPoint == null){
                    targetPoint = activeSprite.gameObject.transform;
                }
                phaseOne();
            }
        }
    }

    private void phaseOne(){
        if(activeCounter > 0 && targetPoint == null){
            activeCounter -= Time.deltaTime;
            fadeCounter = fadeOutTime;
        }
        else if (activeCounter > 0){
            if(Vector3.Distance(activeSprite.gameObject.transform.position, targetPoint.transform.position) > 0.02f){
                activeSprite.gameObject.transform.position = Vector3.MoveTowards(activeSprite.gameObject.transform.position, targetPoint.transform.position, moveSpeed * Time.deltaTime);
            } else {
                activeCounter = 0;
                fadeCounter = fadeOutTime;
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

    private void TransitionPhase(int phaseNumber){
        if(!PhaseTwoTransition && sprites.Length >= phaseNumber && inactiveCounter > 0 ){
            int index = phaseNumber - 1;
            Transform currentPoint = activeSprite.gameObject.transform;
            activeSprite.gameObject.SetActive(false);
            activeSprite = sprites[index];
            getAnimator();
            activeSprite.transform.position = currentPoint.position;
            PhaseTwoTransition = true;
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
        yield return new WaitForSeconds(TimeBeforeCombatStart);
        activeSprite.gameObject.SetActive(true);
        getAnimator();
        battleStarted = true;
        activeCounter = activeTime;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Animator anim;
    public float distanceToOpen;
    private PlayerController thePlayer;
    private bool playerExiting;
    private const float EXITTIME = 1f;
    public Transform exitPoint;
    public float movePlayerSpeed;
    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = PlayerHealthController.instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, thePlayer.transform.position) < distanceToOpen)
        {
            anim.SetBool("doorOpen", true);
        } else
        {
            anim.SetBool("doorOpen", false);
        }

        if (playerExiting){
            thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,exitPoint.position,movePlayerSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !playerExiting){
            thePlayer.canMove = false;
            StartCoroutine(UseDoorCoRoutine());
        }
    }

    IEnumerator UseDoorCoRoutine()
    {
        playerExiting = true;
        thePlayer.animator.enabled = false;
        UIController.instance.StartFadeToBlack();
        yield return new WaitForSeconds(EXITTIME);

        RespawnController.instance.SetSpawn(exitPoint.position);
        thePlayer.canMove = true;
        thePlayer.animator.enabled = true;
        UIController.instance.StartFadeFromBlack();
        SceneManager.LoadScene(levelToLoad);
    }
}

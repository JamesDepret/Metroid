using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawnerController : MonoBehaviour
{
    // Start is called before the first frame update
    public FlameBall flameBall;
    public Transform shotPoint;
    public int shotsAmount;
    private int shotCounter;
    public float fireWaitTime = 1f;
    private float waitCounter;
    public float moveSpeed;
    public Animator anim;
    private float fadeCounter;
    void Start()
    {
        transform.position = new Vector3(PlayerHealthController.instance.transform.position.x, 8.2f);
        waitCounter = fireWaitTime;
        shotCounter = shotsAmount;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(PlayerHealthController.instance.transform.position.x, transform.position.y) ;
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
        if(shotCounter >= 0)
        {
            fire();
            fadeCounter = 0.5f;
        } else
        {
            anim.SetTrigger("vanish");
            if(fadeCounter <= 0) gameObject.SetActive(false);
            fadeCounter -= Time.deltaTime;
        }
    }

    private void fire()
    {
        waitCounter -= Time.deltaTime;
        if(waitCounter <= 0)
        {
            Instantiate(flameBall, shotPoint.position, shotPoint.rotation);
            waitCounter = fireWaitTime;
            shotCounter--;
        }
    }
}

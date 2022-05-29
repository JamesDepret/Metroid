using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBulletCircle : MonoBehaviour
{

    public MageSimpleBullet shotToFire;
    public int shotsAmount;
    public float shotDistance;
    private int shotsFired;
    private float shotAngle;
    private float currentAngle;
    private bool rotationDirection;
    public float fireWaitTime = 0.33f;

    private void Start()
    {
    }
    public void FireCircle()
    {
        shotsFired = 0;
        shotAngle = 180 / shotsAmount;
        currentAngle = Random.Range(0,360);
        shotAngle = Random.Range(0, 2) == 0 ? shotAngle * -1 : shotAngle;
        StartCoroutine(ShotsCoRoutine());
    }

    IEnumerator ShotsCoRoutine()
    {
        yield return new WaitForSeconds(fireWaitTime);
        currentAngle += shotAngle;
        Vector3 position = new Vector3(transform.position.x + shotDistance * Mathf.Sin((currentAngle * Mathf.PI) / 180), transform.position.y + shotDistance * Mathf.Cos((currentAngle * Mathf.PI) / 180));
        Instantiate(shotToFire, position, transform.rotation).moveDir = new Vector2(Mathf.Sin((currentAngle * Mathf.PI) / 180), Mathf.Cos((currentAngle * Mathf.PI) / 180));
        currentAngle += shotAngle;
        if(shotsFired < shotsAmount)
        {
            shotsFired++;
            StartCoroutine(ShotsCoRoutine());
        }
    }

    public void UpdateLocation(Transform location)
    {
        transform.position = location.position;
    }
}

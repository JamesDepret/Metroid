using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBattle : MonoBehaviour
{
    private CameraController Camera;
    public Transform CamPosition;
    public float CamSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Camera = FindObjectOfType<CameraController>();
        Camera.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, CamPosition.position, CamSpeed * Time.deltaTime);
    }
}

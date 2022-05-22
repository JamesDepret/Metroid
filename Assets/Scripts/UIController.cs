using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Image fadeScreen;
    public float fadeSpeed = 2;
    private bool fadingToBlack, fadingFromBlack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Fading();
    }

    private void Fading(){
        if(fadingToBlack) {
            fadeScreen.color = new Color(fadeScreen.color.r,fadeScreen.color.g,fadeScreen.color.b,Mathf.MoveTowards(fadeScreen.color.a,1f,fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 1f) {
                fadingToBlack = false;
            }
        } else if (fadingFromBlack) {
            fadeScreen.color = new Color(fadeScreen.color.r,fadeScreen.color.g,fadeScreen.color.b,Mathf.MoveTowards(fadeScreen.color.a,0f,fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f) {
                fadingFromBlack = false;
            }
        }
    }

    public void UpdateHealth(int currentHealth, int maxhealth)
    {
        healthSlider.maxValue = maxhealth;
        healthSlider.value = currentHealth;
    }

    public void StartFadeToBlack(){
        fadingToBlack = true;
        fadingFromBlack = false;
    }
    public void StartFadeFromBlack(){
        fadingFromBlack = true;
        fadingToBlack = false;
        
    }
}

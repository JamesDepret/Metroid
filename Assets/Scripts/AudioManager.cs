using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

    }
    public AudioSource mainMenuMusic, levelMusic, bossMusic;
    public AudioSource[] sfx;
    // Start is called before the first frame update
    
    public void PlayMainMenuMusic()
    {
        levelMusic.Stop();
        bossMusic.Stop();
        mainMenuMusic.Play();
    }

    public void PlayLevelMusic()
    {
        if (!levelMusic.isPlaying)
        {
            mainMenuMusic.Stop();
            levelMusic.Stop();
            levelMusic.Play();
        }
    }

    public void PlayBossMusic()
    {
        mainMenuMusic.Stop();
        levelMusic.Stop();
        bossMusic.time = 7;
        bossMusic.Play();
    }
}

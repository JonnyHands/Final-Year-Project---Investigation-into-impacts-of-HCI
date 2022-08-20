using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public AudioSource gameMusic, deathMusic, endMusic, bossMusic;
    public AudioSource[] soundEffects;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame


    //plays main game music
    public void PlayGameMusic()
    {
        gameMusic.Play();
        bossMusic.Stop();
        deathMusic.Stop();
    }
    //plays music for death screen
    public void PlayDeathMusic()
    {
        gameMusic.Stop();
        bossMusic.Stop();
        deathMusic.Play();
    }
    //plays credits music
    public void PlayEndMusic()
    {
        gameMusic.Stop();
        endMusic.Play();
    }
    //plays boss music
    public void PlayBossMusic()
    {
        bossMusic.Play();
        gameMusic.Stop();
        deathMusic.Stop();
    }

  
    //selects sound effects to play from list of sound effects
    public void PlaySFX(int soundEffectsToPlay)
    {
        soundEffects[soundEffectsToPlay].Stop();
        soundEffects[soundEffectsToPlay].Play();
    }


}

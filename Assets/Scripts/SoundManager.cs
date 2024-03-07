using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sounds { get; private set; }
    public AudioSource musicSource;
    public AudioSource src1, src2, src3;
    public AudioClip jumpSmall, bump, breakBlock, levelUp, flagPole, coin, powerUpApear, kill, stageClear, gameOver, die, losePowerUp;
    private AudioSource[] audioSources;


    private void Awake()
    {
        if (sounds != null)
        {
            return;
        }
        sounds = this;
        audioSources = new AudioSource[3];
        audioSources[0] = src1;
        audioSources[1] = src2;
        audioSources[2] = src3;
    }

     public void PlayJump()
    {
        AudioSource src = lookForEmptySource();
        src.clip = jumpSmall;
        src.Play();
    }
    public void PlayBump()
    {
        AudioSource src = lookForEmptySource();
        src.clip = bump;
        src.Play();
    }
    public void PlayBreakBlock()
    {
        AudioSource src = lookForEmptySource();
        src.clip = breakBlock;
        src.Play();
    }
    public void PlayLevelUp()
    {
        AudioSource src = lookForEmptySource();
        src.clip = levelUp;
        src.Play();
    }
    public void PlayFlapPole()
    {
        AudioSource src = lookForEmptySource();
        src.clip = flagPole;
        src.Play();
    }
    public void PlayCoin()
    {
        AudioSource src = lookForEmptySource();
        src.clip = coin;
        src.Play();
    }
   
    public void PlayPowerupAppear()
    {
        AudioSource src = lookForEmptySource();
        src.clip = powerUpApear;
        src.Play();
    }
    public void PlayStageClear()
    {
        AudioSource src = lookForEmptySource();
        StopAllSounds();
        print("play");
        src.clip = stageClear;
        src.Play();
    }
    public void PlayGameOver()
    {
        StopAllSounds();
        AudioSource src = lookForEmptySource();
        src.clip = gameOver;
        src.Play();
    }

 

    public void PlayDie()
    {
        AudioSource src = lookForEmptySource(); 
        src.clip = die;
        src.Play();
    }

    public void PlayKill()
    {
        AudioSource src = lookForEmptySource();
        src.clip = kill;
        src.Play();
    }
    public void PlayLosePowerup()
    {
        AudioSource src = lookForEmptySource();
        src.clip = losePowerUp;
        src.Play();
    }


    private AudioSource lookForEmptySource()
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return audioSources[0];
    }
    private void StopAllSounds()
    {
        foreach (var source in audioSources)
        {
            if (!source.clip != die)
            {
                source.Stop();
            }
        }
        musicSource.Stop();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEventSounds : MonoBehaviour
{
    //Audio Sources
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    public AudioClip battleMusic;
    public AudioClip victoryMusic;

    public AudioClip rumbleSound;
    public AudioClip platformBreakSound1;
    public AudioClip platformBreakSound2;
    public AudioClip thirtySecondsLeftSound;
    public AudioClip tenSecondsLeftSound;
    public AudioClip finishSound;
    public AudioClip finishVoiceSound;
    public AudioClip victorySound;

    //Event music methods
    public void playBattleMusic()
    {
        musicAudioSource.PlayOneShot(battleMusic);
    }

    public void playVictoryMusic()
    {
        musicAudioSource.PlayOneShot(victoryMusic);
    }

    //Event sound methods
    public void playPlatformBreak1Sound()
    {
        sfxAudioSource.PlayOneShot(platformBreakSound1);
    }

    public void playPlatformBreak2Sound()
    {
        sfxAudioSource.PlayOneShot(platformBreakSound2);
    }

    public void playthirtySecondsLeftSound()
    {
        sfxAudioSource.PlayOneShot(thirtySecondsLeftSound);
    }

    public void playtenSecondsLeftSound()
    {
        sfxAudioSource.PlayOneShot(tenSecondsLeftSound);
    }

    public void playRumbleSound()
    {
        sfxAudioSource.PlayOneShot(rumbleSound);
    }

    public void playfinishSound()
    {
        sfxAudioSource.PlayOneShot(finishSound);
        sfxAudioSource.PlayOneShot(finishVoiceSound);
    }

    public void playVictorySound()
    {
        sfxAudioSource.PlayOneShot(victorySound);
    }
}

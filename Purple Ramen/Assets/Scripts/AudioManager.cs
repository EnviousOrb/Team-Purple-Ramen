using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public soundObject[] BGM, SFX, NpcSFX, PlayerSFX;
    public AudioSource BGMSource, SFXSource,NPCSource, PlayerSource;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playBGM("Thug's Hangout");
    }

    public void playBGM(string name)
    {
        soundObject so = Array.Find(BGM, x => x.name == name);

        if (so == null)
        {
            Debug.Log("Background Music Not Found");
        }
        else
        {
            BGMSource.clip = so.soundClip;
            BGMSource.Play();
        }
    }

    public void playPlayerSFX(string name)
    {
        soundObject so = Array.Find(PlayerSFX, x => x.name == name);

        if (so == null)
        {
            Debug.Log("Sound Effect Not Found");
        }
        else
        {
            PlayerSource.PlayOneShot(so.soundClip);
        }
    }

    public void playSFX(string name)
    {
        soundObject so = Array.Find(SFX, x => x.name == name);

        if (so == null)
        {
            Debug.Log("Sound Effect Not Found");
        }
        else
        {
            SFXSource.PlayOneShot(so.soundClip);
        }
    }


    public void playNpcSFX(string name)
    {
        soundObject so = Array.Find(NpcSFX, x => x.name == name);

        if (so == null)
        {
            Debug.Log("NPC Sound Effect Not Found");
        }
        else
        {
            NPCSource.PlayOneShot(so.soundClip);
        }
    }

    public void toggleBGM()
    {
        BGMSource.mute = !BGMSource.mute;
    }

    public void stopAll()
    {
        BGMSource.Stop();
        PlayerSource.Stop();
        NPCSource.Stop();
    }

    /*public void toggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }*/

    public void bgmVolume(float volume)
    {
        BGMSource.volume = volume;
    }

    /*public void sfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    [SerializeField] SceneInfo sceneInfo;
    public static AudioManager instance;
    public soundObject[] BGM, SFX, NpcSFX, PlayerSFX, BossSFX, EnemySFX;
    public float fadeDuration;
    public float fadeVolume;
    public AudioSource BGMSource, SFXSource,NPCSource, BossSource, PlayerSource, EnemySource;

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
        playBGM(BGM[0].soundName);
        PlayerSource = GameObject.FindWithTag("Player")?.GetComponent<AudioSource>();
        BossSource = GameObject.FindWithTag("Boss")?.GetComponent<AudioSource>();
        BossSource = GameObject.FindWithTag("Boss")?.GetComponent<AudioSource>();
    }

    private void Update()
    {
        EnemySource = GameObject.FindWithTag("Enemy")?.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Main Area"))
        {
            StartCoroutine(FadeAway(BGMSource, fadeDuration, fadeVolume));
            playBGM(BGM[1].soundName);
        }
        else if(other.gameObject.CompareTag("Miniboss Area"))
        {
            StartCoroutine(FadeAway(BGMSource, fadeDuration, fadeVolume));
            playBGM(BGM[2].soundName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Miniboss Area"))
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator FadeAway(AudioSource AS, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = AS.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            AS.volume = Mathf.Lerp(start,targetVolume,currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void playBGM(string name)
    {
        soundObject so = Array.Find(BGM, x => x.name == name);
            if (so == null)
            {
                // Debug.Log("Background Music Not Found");
            }
            else
            {
                if (BGMSource.isPlaying)
                {
                    BGMSource.Stop();
                }

                BGMSource.clip = so.soundClip;
                BGMSource.Play();
            }
    }

    public void playPlayerSFX(string name)
    {
        soundObject so = Array.Find(PlayerSFX, x => x.name == name);

        if (so == null)
        {
            // Debug.Log("Sound Effect Not Found");
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

    public void playBossSFX(string name)
    {
        soundObject so = Array.Find(BossSFX, x => x.name == name);

        if (so == null)
        {
            Debug.Log("Sound Effect Not Found");
        }
        else
        {
            BossSource.PlayOneShot(so.soundClip);
        }
    }

    public void playEnemySFX(string name)
    {
        soundObject so = Array.Find(EnemySFX, x => x.name == name);

        if (so == null)
        {
            Debug.Log("Sound Effect Not Found");
        }
        else
        {
            EnemySource.PlayOneShot(so.soundClip);
        }
    }


    public void playNpcSFX(string name)
    {
        soundObject so = Array.Find(NpcSFX, x => x.name == name);

        if (so == null)
        {
            // Debug.Log("NPC Sound Effect Not Found");
        }
        else
        {
            NPCSource.PlayOneShot(so.soundClip);
        }
    }

    public void stopAll()
    {
        BGMSource.Stop();
        PlayerSource.Stop();
        if(NPCSource != null)
            NPCSource.Stop();
        if(BossSource != null)
            BossSource.Stop();
        if(EnemySource != null) 
            EnemySource.Stop();
    }

    public void toggleBGM()
    {
        BGMSource.mute = !BGMSource.mute;
    }

    public void toggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
        PlayerSource.mute = !PlayerSource.mute;
        if (BossSource != null)
            BossSource.mute = !BossSource.mute;
        if (EnemySource != null)
            EnemySource.mute = !EnemySource.mute;
    }

    public void toggleNPC()
    {
        NPCSource.mute = !NPCSource.mute;
    }

    public void bgmVolume(float volume)
    {
        BGMSource.volume = volume;
    }

    public void sfxVolume(float volume)
    {
        SFXSource.volume = volume;
        PlayerSource.volume = volume;
        if(BossSource != null)
            BossSource.volume = volume;
    }

    public void NPCVolume(float volume)
    {
        NPCSource.volume = volume;
    }
}

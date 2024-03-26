using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource AS;
    [SerializeField] AudioClip[] chestMusic;
    [SerializeField] AudioClip[] playerSteps;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip bonfireMusic;
    [SerializeField] AudioClip NPCTalk;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Debug.Log(AS == null ? "AudioSource is null" : "AudioSource is not null");
            Debug.Log(AS.enabled ? "AudioSource is enabled" : "AudioSource is disabled");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioClip PlayChestMusic()
    {
        return chestMusic[Random.Range(0, chestMusic.Length)];
    }
}

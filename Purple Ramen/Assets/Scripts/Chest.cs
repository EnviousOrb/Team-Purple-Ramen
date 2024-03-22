using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] List<ItemData> chestList;
    [SerializeField] AudioClip[] chestAudio;
    private Animator animate;
    private AudioSource AS;
    private void Awake()
    {
        animate = GetComponent<Animator>();
        animate.SetBool("isInRange",false);
        AS = GetComponent<AudioSource>();
        StartCoroutine(AudioLoop());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animate.SetBool("isInRange",true);
        }
    }

    public void DeleteChest()
    {
        GiveItem();
        Destroy(gameObject);
    }

    private IEnumerator AudioLoop()
    {
        int clipIndex = 0;
        while (true)
        {
            AS.clip = chestAudio[clipIndex];
            AS.Play();
            yield return new WaitForSeconds(chestAudio[clipIndex].length);
            clipIndex = (clipIndex + 1) % chestAudio.Length;
        }
    }

    public void GiveItem()
    {
        // Give the item
        ItemData randomItem = chestList[Random.Range(0, chestList.Count)];
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ItemData newItem = randomItem.Clone();
        gameManager.instance.PS.GetItem(newItem);
    }
}

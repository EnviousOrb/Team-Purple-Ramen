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
        // Directly use an item from the list without cloning
        if (chestList.Count > 0)
        {
            List<ItemData> itemsNotOwned = chestList.FindAll(item => !gameManager.instance.PS.ItemList.Contains(item));
            if (itemsNotOwned.Count > 0)
            {
                ItemData randomItem = itemsNotOwned[Random.Range(0, itemsNotOwned.Count)];
                Debug.Log($"Awarding item: {randomItem.name}");
                gameManager.instance.PS.GetItem(randomItem);
            }
            else
            {
                Debug.Log("Player already has all items.");
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] List<ItemData> chestList; // List of possible items to be awarded from this chest
    [SerializeField] AudioClip[] chestAudio; // Array of audio clips for chest interactions
    private Animator animate;
    private AudioSource AS;

    private void Awake()
    {
        animate = GetComponent<Animator>();
        animate.SetBool("isInRange", false);
        AS = GetComponent<AudioSource>();
        StartCoroutine(AudioLoop());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger animation and possibly give item when player is in range
        if (other.gameObject.CompareTag("Player"))
        {
            animate.SetBool("isInRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optionally, revert any changes when the player leaves the chest's range
        if (other.gameObject.CompareTag("Player"))
        {
            animate.SetBool("isInRange", false);
        }
    }

    public void DeleteChest()
    {
        GiveItem();
        Destroy(gameObject); // Destroy the chest object after giving an item
    }

    private IEnumerator AudioLoop()
    {
        // Loop through chest audio clips
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
        // Check if there are items in the chest that the player doesn't already have
        if (chestList.Count > 0)
        {
            List<ItemData> itemsNotOwned = chestList.FindAll(item => !gameManager.instance.PS.ItemList.Contains(item));
            if (itemsNotOwned.Count > 0)
            {
                // Select a random item from those the player does not have and add it to their inventory
                ItemData randomItem = itemsNotOwned[Random.Range(0, itemsNotOwned.Count)];
                Debug.Log($"Awarding item: {randomItem.name}"); // Confirm the awarded item
                gameManager.instance.PS.GetItem(randomItem);
            }
            else
            {
                Debug.Log("Player already has all items available from this chest.");
            }
        }
    }
}


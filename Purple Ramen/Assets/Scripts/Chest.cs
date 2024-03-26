using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] List<ItemData> chestList; // List of possible items to be awarded from this chest
    private Animator animate;
    private AudioSource AS;
    public void Awake()
    {
        animate = GetComponent<Animator>();
        animate.SetBool("isInRange", false);
        AS = gameObject.AddComponent<AudioSource>();
        AS.clip = AudioManager.instance.PlayChestMusic();
        Debug.Log(AS.clip == null ? "Chest music clip is null" : "Chest music clip is not null");
        AS.loop = true;
        AS.Play();
    }
    private void Update()
    {
        if (AS.isPlaying)
        {
            Debug.Log("Chest music is playing");
        }
        else
        {
            Debug.Log("Chest music is not playing");
        }
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
        AS.Stop();
        Destroy(gameObject); // Destroy the chest object after giving an item
    }

    public void GiveItem()
    {
        // Check if there are items in the chest that the player doesn't already have
        if (chestList.Count > 0)
        {
            List<ItemData> itemsNotOwned = chestList.FindAll(item => !gameManager.instance.PS.itemList.Contains(item));
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


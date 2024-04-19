using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] List<ItemData> chestList; // List of possible items to be awarded from this chest
    [SerializeField] List<staffElementalStats> staffList;
    [SerializeField] AudioClip[] chestAudio; // Array of audio clips for chest interactions
    [SerializeField] GameObject chestLight;
    [SerializeField] bool randomReward = true;

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

    public void chestLightON()
    {
        chestLight.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animate.SetBool("isInRange", false);
            gameManager.instance.HideTextBox();
        }
    }

    public void DeleteChest()
    {
        GiveReward();
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

    public void GiveReward()
    {
        bool itemGiven = false;

        if (staffList.Count > 0)
        {
            staffElementalStats selectedStaff = staffList[Random.Range(0, staffList.Count)];
            gameManager.instance.PS.getStaffStats(selectedStaff);
            gameManager.instance.UpdateTextBox("You've received a magical staff orb!");
            itemGiven = true;
        }

        if (!itemGiven && chestList.Count > 0)
        {
            List<ItemData> itemsNotOwned = chestList.FindAll(item => !gameManager.instance.PS.itemList.Contains(item));
            if (itemsNotOwned.Count > 0)
            {
                ItemData randomItem = itemsNotOwned[Random.Range(0, itemsNotOwned.Count)];
                gameManager.instance.PS.GetItem(randomItem);
                gameManager.instance.UpdateTextBox("You've received..." + randomItem.name);
                itemGiven = true;
            }
        }
    }
}


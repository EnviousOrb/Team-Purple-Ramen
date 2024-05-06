using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class catScript : MonoBehaviour
{
    [SerializeField] private ItemData requiredItem; // The item the cat wants
    [SerializeField] public GameObject GateToUnlock;
    [SerializeField] public GameObject WallToUnlock;
    [SerializeField] private SuperTextMesh questText;
    [SerializeField] private SuperTextMesh thankText;
    private bool itemGiven = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player != null)
            {
                PlayRandomCatSound();

                if (!itemGiven && player.itemList.Contains(requiredItem))
                {
                    player.itemList.Remove(requiredItem);

                    UIManager.instance.UpdateInventoryUI(player.itemList);

                    if (GateToUnlock != null)
                        GateToUnlock.SetActive(true);
                    if (WallToUnlock != null)
                        WallToUnlock.SetActive(false);

                    gameManager.instance.UpdateTextBox(thankText.text, 15);
                    itemGiven = true;
                }
                else if (itemGiven)
                {
                    gameManager.instance.UpdateTextBox(thankText.text, 15);
                }
                else
                {
                    gameManager.instance.UpdateTextBox(questText.text, 15);
                }
            }
        }
    }

    private void PlayRandomCatSound()
    {
        int randomIndex = Random.Range(0, AudioManager.instance.NpcSFX.Length);
        string randomSFXName = AudioManager.instance.NpcSFX[randomIndex].name;
        AudioManager.instance.playNpcSFX(randomSFXName);
    }

    private void OnTriggerExit(Collider other)
    {
        gameManager.instance.HideTextBox();
    }
}


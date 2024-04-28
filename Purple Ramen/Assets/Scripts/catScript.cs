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
    bool isSpeaking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player != null && player.itemList.Contains(requiredItem))
            {
                // Remove the item from the player's inventory
                player.itemList.Remove(requiredItem);

                // Update the inventory UI to reflect this change
                UIManager.instance.UpdateInventoryUI(player.itemList);

                if(GateToUnlock != null)
                    GateToUnlock.SetActive(true);
                if(WallToUnlock != null)
                    WallToUnlock.SetActive(false);
                gameManager.instance.UpdateTextBox(thankText.text);

            }
            else
            {
                // Player doesn't have the required item. Just show the regular cat menu.
                gameManager.instance.UpdateTextBox(questText.text);
                StartCoroutine(CatSpeak());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // You might want to deactivate the win menu here as well if you're using OnTriggerExit to close the catMenu.
            gameManager.instance.HideTextBox();
        }
    }

    IEnumerator CatSpeak()
    {
        isSpeaking = true;
        int randomIndex = UnityEngine.Random.Range(0, AudioManager.instance.NpcSFX.Length);
        string randomSFXName = AudioManager.instance.NpcSFX[randomIndex].name;
        AudioManager.instance.playNpcSFX(randomSFXName);
        yield return new WaitForSeconds(0.3f);
        isSpeaking = false;
    }
}


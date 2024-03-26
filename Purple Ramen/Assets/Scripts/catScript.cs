using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catScript : MonoBehaviour
{
    [SerializeField] private ItemData requiredItem; // The item the cat wants

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player != null && player.itemList.Contains(requiredItem))
            {
                // Player has the required item. Trigger the win condition.
                gameManager.instance.stateWin();

                // Remove the item from the player's inventory
                player.itemList.Remove(requiredItem);

                // Update the inventory UI to reflect this change
                UIManager.instance.UpdateInventoryUI(player.itemList);
            }
            else
            {
                // Player doesn't have the required item. Just show the regular cat menu.
                gameManager.instance.catMenu.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // You might want to deactivate the win menu here as well if you're using OnTriggerExit to close the catMenu.
            gameManager.instance.catMenu.SetActive(false);
        }
    }
}


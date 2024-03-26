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
                gameManager.instance.UpdateTextBox("Hiya Friend! There are 6 chest in this dungeon that contains ingredients for my favorite dish, gather them and bring them to the fire next to me! Then, bring back the deliiiicious meal to me! Nyaa~ :3");
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
}


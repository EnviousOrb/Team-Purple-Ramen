using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] recipeManager recipe; // Assume we've added a Recipe variable here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(UseBonfire(other.GetComponent<playerController>()));
        }
    }

    IEnumerator UseBonfire(playerController player)
    {
        if (recipe.requiredItems.All(requiredItem => player.itemList.Contains(requiredItem)))
        {
            foreach (var item in recipe.requiredItems)
            {
                player.itemList.Remove(item); // Remove each required item
            }

            player.GetItem(recipe.resultItem); // Add the reward item
            UIManager.instance.UpdateInventoryUI(player.itemList);

            gameManager.instance.chestMenuGood.SetActive(true);
            yield return new WaitForSeconds(2f);
            gameManager.instance.chestMenuGood.SetActive(false);
        }
        else
        {
            gameManager.instance.chestMenuBad.SetActive(true);
            yield return new WaitForSeconds(2f);
            gameManager.instance.chestMenuBad.SetActive(false);
        }
    }

}


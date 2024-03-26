using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] recipeManager recipe; //serializes a variable from the recipe item

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

            gameManager.instance.BonfireGood.SetActive(true);
            yield return new WaitForSeconds(2f);
            gameManager.instance.BonfireGood.SetActive(false);
        }
        else
        {
            gameManager.instance.BonfireBad.SetActive(true);
            yield return new WaitForSeconds(2f);
            gameManager.instance.BonfireBad.SetActive(false);
        }
    }

}


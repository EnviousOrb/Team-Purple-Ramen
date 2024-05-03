using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] recipeManager recipe; //serializes a variable from the recipe item
    private bool recipeCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(UseBonfire(other.GetComponent<playerController>()));
        }
    }

    IEnumerator UseBonfire(playerController player)
    {
        if (recipeCompleted)
        {
            gameManager.instance.UpdateTextBox("You've already cooked " + recipe.resultItem.name);
            yield return new WaitForSeconds(3f);
            gameManager.instance.HideTextBox();
        }
        else if (recipe.requiredItems.All(requiredItem => player.itemList.Contains(requiredItem)))
        {
            foreach (var item in recipe.requiredItems)
            {
                player.itemList.Remove(item); 
            }

            player.GetItem(recipe.resultItem); 
            UIManager.instance.UpdateInventoryUI(player.itemList);

            gameManager.instance.UpdateTextBox("You've received " + recipe.resultItem.name);
            yield return new WaitForSeconds(3f);
            gameManager.instance.HideTextBox();
            recipeCompleted = true; 
        }
        else
        {
            gameManager.instance.UpdateTextBox("You're missing some required items...");
            yield return new WaitForSeconds(3f);
            gameManager.instance.HideTextBox();
        }
    }

    public void ResetRecipe()
    {
        recipeCompleted = false;
    }
}


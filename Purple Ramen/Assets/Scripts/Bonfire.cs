using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] ItemData rewardItem;
    [SerializeField] public List<ItemData> itemListCheck = new List<ItemData>();
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(useBonfire());

        }
    }
    IEnumerator useBonfire()
    {
        if (itemListCheck.All(item => gameManager.instance.PS.ItemList.Contains(item)))
        {
            gameManager.instance.PS.ItemList.Clear();
            gameManager.instance.PS.GetItem(rewardItem);

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

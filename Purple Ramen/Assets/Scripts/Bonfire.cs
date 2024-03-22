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
        useBonfire();
    }
    void useBonfire()
    {
        if (gameManager.instance.PS.ItemList.All(item => itemListCheck.Contains(item)))
        {
            gameManager.instance.PS.ItemList.Clear();
            gameManager.instance.PS.ItemList.Add(rewardItem);
            Debug.Log("You recieved: " + rewardItem.name);
        }
        else
        {
            Debug.Log("You don't have the required items.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] List<ItemData> chestList;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GiveItem();
            Destroy(gameObject);
        }
    }

    private void GiveItem()
    {
        ItemData randomItem = chestList[Random.Range(0, chestList.Count)];
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController PC = gameManager.instance.PS;
        ItemData newItem = randomItem.Clone();
        PC.ItemList.Add(newItem);
    }
}

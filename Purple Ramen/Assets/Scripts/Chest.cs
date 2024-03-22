using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] List<ItemData> chestList;
    private Animator animate;
    private void Awake()
    {
        animate = GetComponent<Animator>();
        animate.SetBool("isInRange",false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animate.SetBool("isInRange",true);
        }
    }

    public void DeleteChest()
    {
        GiveItem();
        Destroy(gameObject);
    }

    public void GiveItem()
    {
        // Give the item
        ItemData randomItem = chestList[Random.Range(0, chestList.Count)];
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ItemData newItem = randomItem.Clone();
        gameManager.instance.PS.GetItem(newItem);
    }
}

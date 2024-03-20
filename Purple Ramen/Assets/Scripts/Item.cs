using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemData ID;

    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.PS.GetItem(ID);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.catMenu.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        gameManager.instance.catMenu.SetActive(false);
    }
}

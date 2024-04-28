using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporture : MonoBehaviour
{
    [SerializeField] Transform placeToTeleport;

    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.PS.GetComponent<CapsuleCollider>().transform.position = placeToTeleport.position;
    }

    private void OnTriggerExit(Collider other)
    {
        gameManager.instance.PS.GetComponent<CapsuleCollider>().transform.position = placeToTeleport.position;
    }
}

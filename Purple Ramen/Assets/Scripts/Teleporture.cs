using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporture : MonoBehaviour
{
    [SerializeField] GameObject placeToTeleport;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = placeToTeleport.transform.position;
    }
}

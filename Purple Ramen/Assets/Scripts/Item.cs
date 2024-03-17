using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    [SerializeField] float forceAmount = 10.0f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.tag == "Player")
        {
            // Get the direction from the object to the player
            Vector3 direction = collision.transform.position - transform.position;
            direction = direction.normalized;

            // Apply a force in the direction of the player
            GetComponent<Rigidbody>().AddForce(direction * forceAmount, ForceMode.Impulse);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeDamage : MonoBehaviour
{
    [SerializeField] int damage; // The amount of damage this bullet will deal upon hitting an IDamage interface implementer.

    // Start is called before the first frame update
    void Start()
    {

    }

    // This function is called when the bullet's collider encounters another collider.
    private void OnTriggerEnter(Collider other)
    {
        // Ignore the collision if the other object's collider is marked as a trigger.
        if (other.isTrigger || other.CompareTag("Player"))
            return;

        // Attempts to get an IDamage interface from the collided object.
        IDamage dmg = other.GetComponent<IDamage>();


        // If the other object implements IDamage, it calls takeDamage() on it with this bullet's damage value.
        Debug.Log(other.gameObject.name + " : None");
        if (dmg != null)
        {
            Debug.Log(other.gameObject.name + " : Has Damage");
            dmg.takeDamage(damage);
        }
    }
}

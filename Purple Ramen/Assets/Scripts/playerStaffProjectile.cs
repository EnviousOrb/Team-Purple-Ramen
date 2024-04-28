using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the behavior of a bullet in the game.
public class playerStaffProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb; // Reference to the Rigidbody component for physics operations.
    [SerializeField] int damage; // The amount of damage this bullet will deal upon hitting an IDamage interface implementer.
    [SerializeField] float speed; // The speed at which the bullet moves.
    [SerializeField] int lifespan; // How long (in seconds) the bullet exists before automatically being destroyed.

    // Start is called before the first frame update
    void Start()
    {
        // Sets the bullet's velocity in the direction it's facing multiplied by its speed.
        rb.velocity = transform.forward * speed;
        // Automatically destroys the bullet after 'lifespan' seconds to prevent it from existing indefinitely.
        Destroy(gameObject, lifespan);
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
        // Debug.Log(other.gameObject.name + " : None");
        if (dmg != null)
        {
            // Debug.Log(other.gameObject.name + " : Has Damage");
            dmg.takeDamage(damage, 0);
        }

        // Destroys the bullet upon hitting something to simulate it being "spent".
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This class controls enemy AI behavior in the game.
public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model; // The Renderer component of the enemy model, used to change its appearance.
    [SerializeField] NavMeshAgent agent; // Reference to the NavMeshAgent component for navigation.
    [SerializeField] Transform shootPos; // The position from which bullets are fired.

    [SerializeField] int HP; // Health points of the enemy.
    [SerializeField] int speed; // Movement speed of the enemy.

    [SerializeField] GameObject bullet; // Prefab of the bullet that the enemy shoots.
    [SerializeField] float shootRate; // How often the enemy can shoot.

    bool isShooting; // Tracks whether the enemy is currently shooting.

    // Start is called before the first frame update
    void Start()
    {
        // Register this enemy with the game manager to update the game's goal.
        Game_Manager.instance.UpdateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        // Set the enemy's destination to the player's current position.
        agent.SetDestination(Game_Manager.instance.player.transform.position);

        // If not already shooting, start the shooting coroutine.
        if (!isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    // Coroutine to handle shooting.
    IEnumerator shoot()
    {
        isShooting = true;
        // Create a bullet at the shooting position.
        Instantiate(bullet, shootPos.position, transform.rotation);
        // Wait for a period equal to shootRate before allowing the enemy to shoot again.
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    // This method is called when the enemy takes damage.
    public void takeDamage(int amount)
    {
        HP -= amount; // Decrease HP by the damage amount.
        StartCoroutine(flashRed()); // Flash the model red to indicate damage.

        // If HP drops to 0 or below, update the game goal and destroy the enemy game object.
        if (HP <= 0)
        {
            Game_Manager.instance.UpdateEnemyCount(-1);
            Destroy(gameObject);
        }
    }

    // Coroutine to flash the enemy's color to red when damaged.
    IEnumerator flashRed()
    {
        model.material.color = Color.red; // Change color to red.
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds.
        model.material.color = Color.white; // Change color back to white.
    }
}

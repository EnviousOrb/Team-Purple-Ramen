using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] int stoppingDist;

    [SerializeField] GameObject projectile; // Prefab of the bullet that the enemy shoots.
    [SerializeField] float shootRate; // How often the enemy can shoot.

    Material originalColor;
    bool isShooting; // Tracks whether the enemy is currently shooting.
    bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        // Register this enemy with the game manager to update the game's goal.
        gameManager.instance.UpdateEnemyCount(1);
        originalColor = model.material;
        agent.speed = speed;
        agent.stoppingDistance = stoppingDist;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the enemy's destination to the player's current position.
        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
            transform.forward = gameManager.instance.player.transform.position;
            if (!isShooting)
            {
                StartCoroutine(shoot());
            }

        }
        // If not already shooting, start the shooting coroutine.
    }

    // Coroutine to handle shooting.
    IEnumerator shoot()
    {
        isShooting = true;
        // Create a bullet at the shooting position.
        Vector3 playerPos = gameManager.instance.player.transform.position;
        playerPos.y = playerPos.y - 1;
        Vector3 playerdirection =  playerPos - transform.position;
        Instantiate(projectile, shootPos.position, Quaternion.LookRotation(playerdirection));
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
            Destroy(gameObject);
            gameManager.instance.UpdateEnemyCount(-1);
        }
    }

    // Coroutine to flash the enemy's color to red when damaged.
    IEnumerator flashRed()
    {
        model.material.color = Color.red; // Change color to red.
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds.
        model.material = originalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}

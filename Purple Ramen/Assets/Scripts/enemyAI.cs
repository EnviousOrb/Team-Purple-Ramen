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
    [SerializeField] int turnSpeed;
    [SerializeField] GameObject bullet; // Prefab of the bullet that the enemy shoots.
    [SerializeField] float shootRate; // How often the enemy can shoot.
    [SerializeField] Material damageMat;
    [SerializeField] int stoppingDist;



    //Variables for canSeePlayer
    [SerializeField] Transform headPos;
    [SerializeField] int viewCone;
    float angleToPlayer;
    Vector3 playerDir;


    //Variables for roam
    bool destinationChosen;
    private float stoppingDistOrig;
    Vector3 startingPos;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    Material originalMat;
    bool isShooting; // Tracks whether the enemy is currently shooting.
    bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        // Register this enemy with the game manager to update the game's goal.
        gameManager.instance.UpdateEnemyCount(1);
        originalMat = model.material;
        agent.stoppingDistance = stoppingDist;
        agent.speed = speed;
        // originalColor = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the enemy's destination to the player's current position.


        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
           

        }
        else if(!playerInRange)
        {
            StartCoroutine(roam());
        }
        // If not already shooting, start the shooting coroutine.
    }

    IEnumerator roam()
    {
        if(agent.remainingDistance<0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
          
    }

    // Coroutine to handle shooting.
    IEnumerator shoot()
    {
        isShooting = true;
        // Create a bullet at the shooting position.
        Vector3 playerDir = gameManager.instance.player.transform.position - transform.position; playerDir.y -= 1;
        Instantiate(bullet, shootPos.position, Quaternion.LookRotation(playerDir));
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
        model.material = damageMat; // Change color to red.
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds.
        model.material = originalMat;
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

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit))
        {
            Debug.Log(hit.collider);

            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                agent.stoppingDistance = stoppingDistOrig;
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                return true;
            }

        }
        agent.stoppingDistance = 0;
        return false;
    }


    void faceTarget()
    {
        Quaternion ROT= Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation=Quaternion.Lerp(transform.rotation,ROT, Time.deltaTime);
    }
}

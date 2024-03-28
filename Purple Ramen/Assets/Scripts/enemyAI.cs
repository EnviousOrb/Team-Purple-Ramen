using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy AI behavior, including movement, shooting, and interactions with the player.
public class enemyAI : MonoBehaviour, IDamage, ISlow
{
    [HeaderAttribute("-----Components-----")]
    [SerializeField] Renderer model; // The enemy's visual model.
    [SerializeField] NavMeshAgent agent; // Navigation component for AI movement.
    [SerializeField] Transform shootPos; // Position from which the enemy shoots.

    [HeaderAttribute("-----Enemy Stats-----")]
    [SerializeField] int HP; // Enemy health points.
    [SerializeField] int speed; // Movement speed.

    [HeaderAttribute("-----Shooting Stats-----")]
    [SerializeField] GameObject bullet; // Bullet prefab for shooting.
    [SerializeField] Material damageMat; // Material to indicate the enemy has taken damage.
    [SerializeField] float shootRate; // Rate at which the enemy shoots.
    [SerializeField] int stoppingDist; // Minimum distance to stop from the player.
    Material originalMat; // The original material of the enemy model.

    [HeaderAttribute("-----Animation-----")]
    [SerializeField] Animator animator; // Animator for controlling enemy animations.
    [SerializeField] int animSpeedTrans; // Speed transition parameter for animations.

    // Variables for checking if the enemy can see the player
    [HeaderAttribute("-----Enemy View-----")]
    [SerializeField] Transform headPos; // Position from which sight checks are made.
    [SerializeField] int viewCone; // The angle of the view cone to detect the player.
    [SerializeField] int faceTargetSpeed; // Speed at which the enemy rotates to face the player.
    [SerializeField] float animatedSpeedTrans; // Speed of transitioning between animations.
    float angleToPlayer; // Angle between the enemy and the player.
    Vector3 playerDir; // Direction from the enemy to the player.

    [HeaderAttribute("-----Enemy Roaming-----")]
    [SerializeField] int roamDist; // Maximum distance the enemy can roam from the start position.
    [SerializeField] int roamPauseTime; // How long the enemy pauses before choosing a new destination.
    bool destinationChosen; // Whether the enemy has chosen a destination to roam to.
    float stoppingDistOrig; // Original stopping distance before any modifications.
    Vector3 startingPos; // Starting position for roaming.

    [HeaderAttribute("-----The Stuffs-----")]
    [SerializeField] int scoreValue;
    public Spawner associatedSpawner;
    int originalSpeed;

    //Bools
    bool isShooting; // Tracks if the enemy is currently shooting.
    bool playerInRange; // Whether the player is within detection range.

    void Start()
    {
        originalMat = model.material; // Stores the original material.
        stoppingDistOrig = agent.stoppingDistance; // Stores the original stopping distance.
        agent.stoppingDistance = 0; // Resets stopping distance for roaming behavior.
        originalSpeed = speed;
    }

    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude; // Calculates speed for animation.
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans)); // Smoothly transitions animation speed.

        // Determines behavior based on player visibility and range.
        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam()); // Starts roaming if player is out of sight but in range.
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam()); // Starts roaming if player is not in range.
        }
    }

    // Coroutine for roaming when the player is not detected.
    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            yield return new WaitForSeconds(roamPauseTime); // Waits before choosing a new destination.

            Vector3 randomPos = Random.insideUnitSphere * roamDist + startingPos; // Chooses a new destination.
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1); // Tries to find a valid point on the NavMesh.
            agent.SetDestination(hit.position); // Sets the new destination.

            destinationChosen = false;
        }
    }

    // Coroutine for shooting at the player.
    IEnumerator shoot()
    {
        isShooting = true;
        animator.SetTrigger("Shoot"); // Triggers the shooting animation.
        Vector3 playerDirection = gameManager.instance.player.transform.position - transform.position;
        playerDirection.y -= 1; // Adjusts bullet spawn height. (this comment is just wrong...)
        Instantiate(bullet, shootPos.position, Quaternion.LookRotation(playerDirection)); // Spawns the bullet.
        bullet.GetComponent<Bullet>().self = this.GetComponentInParent<CapsuleCollider>();
        yield return new WaitForSeconds(shootRate); // Waits before allowing next shot.
        isShooting = false;
    }
    public void getSlowed(float slowModifier, int slowLength)
    {
        StartCoroutine(Slow(slowModifier, slowLength));
    }
    IEnumerator Slow(float slowMod, int slowLength)
    {
        agent.speed = originalSpeed * slowMod;
        yield return new WaitForSeconds(slowLength);
        agent.speed = originalSpeed;
    }

    // Method called when the enemy takes damage.
    public void takeDamage(int amount)
    {
        HP -= amount; // Reduces health by the damage amount.
        StartCoroutine(flashRed()); // Flashes red to indicate damaged.
        // Directs the enemy to move towards the player's position upon taking damage.
        agent.SetDestination(gameManager.instance.player.transform.position);

        // Checks if health has dropped to 0 or below.
        if (HP <= 0)
        {
            //Does something I think ??
            if (associatedSpawner)
                associatedSpawner.UpdateEnemies(-1);
            //Does something else i guess??
            gameManager.instance.playerScore += scoreValue;
            // Destroys the enemy game object.
            Destroy(gameObject);
        }
    }

    // Coroutine to visually indicate damage by changing the enemy's color.
    IEnumerator flashRed()
    {
        model.material = damageMat; // Changes the model's material to the damage indicator.
        yield return new WaitForSeconds(0.1f); // Waits briefly.
        model.material = originalMat; // Restores the original material.
    }

    // Detects when the player enters the enemy's detection range.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Marks that the player is in range.
        }
    }

    // Detects when the player exits the enemy's detection range.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Marks that the player is no longer in range.
            agent.stoppingDistance = 0; // Resets the stopping distance.
        }
    }

    // Determines if the enemy can see the player based on line of sight and angle.
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position; // Calculates direction to the player.
        angleToPlayer = Vector3.Angle(playerDir, transform.forward); // Calculates angle to the player.

        RaycastHit hit;

        // Performs a raycast to check for line of sight to the player.
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            Debug.DrawRay(headPos.position, playerDir, Color.red);
            // Checks if the raycast hit the player and the angle is within the view cone.
            if (hit.collider.CompareTag("Player") || angleToPlayer <= viewCone)
            {
                agent.stoppingDistance = stoppingDistOrig; // Restores the original stopping distance.
                agent.SetDestination(gameManager.instance.player.transform.position); // Moves towards the player.

                // Initiates shooting if not already doing so.
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                // Rotates to face the player if within stopping distance.
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                return true; // Confirms that the player is visible.
            }
        }
        agent.stoppingDistance = 0; // Resets stopping distance if the player is not visible.
        return false; // Indicates that the player is not visible.
    }
    // Rotates the enemy to face the player.
    void faceTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z)); // Calculates the rotation needed to face the player.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * faceTargetSpeed); // Smoothly rotates towards the player.
    }
}

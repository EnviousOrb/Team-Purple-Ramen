using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.VisualScripting;

public class MinibossAI : MonoBehaviour, IDamage
{
    [HeaderAttribute("-----Components-----")]
    [SerializeField] SuperTextMesh minibossName; //The visual representation of the miniboss' name
    [SerializeField] Renderer model; // The enemy's visual model.
    [SerializeField] NavMeshAgent agent; // Navigation component for AI movement.
    [SerializeField] Transform shootPos; // Optional. The place where the Miniboss shoots from
    [SerializeField] GameObject itemToDrop; //Optional. Drops an item from a pre-defined position after enemy dies
    [SerializeField] Collider weaponCollider;
    public Image hpBar; // The physical representation of the miniboss' healthbar

    [HeaderAttribute("-----Miniboss Stats-----")]
    [SerializeField] int HP; // health points.
    [SerializeField] int speed; // Movement speed.

    [HeaderAttribute("-----Shooting Stats-----")]
    [SerializeField] GameObject bullet; // Bullet prefab for shooting.
    [SerializeField] Material damageMat; // Material to indicate taken damage.
    [SerializeField] float shootRate; // Rate at which the miniboss shoots.
    [SerializeField] int stoppingDist; // Minimum distance to stop from the player.
    Material originalMat; // The original material of the model.

    [HeaderAttribute("-----Animation-----")]
    [SerializeField] Animator animator; // Animator for controlling animations.
    [SerializeField] int animSpeedTrans; // Speed transition parameter for animations.

    [HeaderAttribute("-----Miniboss View-----")]
    [SerializeField] Transform headPos; // Position from which sight checks are made.
    [SerializeField] int viewCone; // The angle of the view cone to detect the player.
    [SerializeField] int faceTargetSpeed; // Speed at which the enemy rotates to face the player.
    [SerializeField] float animatedSpeedTrans; // Speed of transitioning between animations.
    float angleToPlayer; // Angle between the enemy and the player.
    Vector3 playerDir; // Direction from the enemy to the player.

    [HeaderAttribute("-----Miniboss Roaming-----")]
    [SerializeField] int roamDist; // Maximum distance the enemy can roam from the start position.
    [SerializeField] int roamPauseTime; // How long the enemy pauses before choosing a new destination.
    bool destinationChosen; // Whether the enemy has chosen a destination to roam to.
    float stoppingDistOrig; // Original stopping distance before any modifications.
    Vector3 startingPos; // Starting position for roaming.

    [HeaderAttribute("-----Miniboss Abilities-----")]
    [SerializeField] bool canSummon;
    [SerializeField] bool canDash;
    [SerializeField] bool canMeleeAttack;
    [SerializeField] bool canShoot;

    [HeaderAttribute("-----Miniboss Abilities Stats-----")]
    [SerializeField] float dashCooldown; //cooldown for dash
    [SerializeField] float dashSpeed; //how fast the enemy dashes
    [SerializeField] float dashDistance; //how far for dash trigger
    [SerializeField] float jumpForce; //How strong the enemy jumps (no relation to the Jump Force game)
    [SerializeField] GameObject enemyToSummon; //what the miniboss summons
    [SerializeField] int maxEnemiesSpawn; //The max amount of enemies the miniboss can spawn
    [SerializeField] Transform minibossPOS; //The position of where the enemy is 
    [SerializeField] float spawnRate; //The rate at which enemies appear
    [SerializeField] float spawnRange; //The range of how far the enemies can spawn

    private Rigidbody rb;
    System.Random rand;
    int enemiesInScene;
    int originalSpeed;
    int ogHealth;
    int action;

    //Bools
    bool isShooting; // Tracks if the enemy is currently shooting.
    bool playerInRange; // Whether the player is within detection range.
    bool isDashing;
    bool isMelee;
    bool isSummoning;
    bool isJumping;

    void Start()
    {
        originalMat = model.material; // Stores the original material.
        stoppingDistOrig = agent.stoppingDistance; // Stores the original stopping distance.
        agent.stoppingDistance = 0; // Resets stopping distance for roaming behavior.
        originalSpeed = speed;
        ogHealth = HP;
    }

    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude; // Calculates speed for animation.
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans)); // Smoothly transitions animation speed.

        hpBar.fillAmount = (float)HP / ogHealth;

        animator.SetBool("playerInRange", true);
        // Determines behavior based on player visibility and range.
        if (playerInRange && !canSeePlayer())
        {
            animator.SetBool("playerInRange", false);
            StartCoroutine(Roam()); // Starts roaming if player is out of sight but in range.
        }
        else if (!playerInRange)
        {
            animator.SetBool("playerInRange", false);
            StartCoroutine(Roam()); // Starts roaming if player is not in range.
        }
    }

    // Coroutine for roaming when the player is not detected.
    IEnumerator Roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            yield return new WaitForSeconds(roamPauseTime); // Waits before choosing a new destination.

            Vector3 randomPos = Random.insideUnitSphere * roamDist + startingPos; // Chooses a new destination.
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1); // Tries to find a valid point on the NavMesh.
            agent.SetDestination(hit.position); // Sets the new destination.

            destinationChosen = false;
        }
    }


    public void MinibossAttack(int action)
    {
        switch (action)
        {
            case 0:
                if (canDash && !isDashing)
                {
                    Dash();
                }
                break;
            case 1:
                if (canShoot && !isShooting)
                {
                    Shoot();
                }
                break;
            case 2:
                if (canMeleeAttack && !isMelee)
                {
                    MeleeAttack();
                }
                break;
            case 3:
                if (canSummon && !isSummoning)
                {
                    Summoning();
                }
                break;
            default:
                break;
        }
    }

    public void Dash()
    {
        isDashing = true;
        float distanceToPlayer = Vector3.Distance(transform.position, gameManager.instance.PS.transform.position);
        if (distanceToPlayer < dashDistance && !isJumping)
        {
            rb.AddForce(-playerDir * jumpForce, ForceMode.Impulse);
            isJumping = true;
            isDashing = false;
        }
        else if(distanceToPlayer > dashDistance && isJumping)
        {
            rb.velocity = playerDir * dashSpeed;
            isJumping = false;
            isDashing = true;
        }
        isDashing = false;
    }

    public void Shoot()
    {
        if (shootPos != null)
        {
            isShooting = true;
            animator.SetTrigger("Shoot"); // Triggers the shooting animation.
            Vector3 playerDirection = gameManager.instance.player.transform.position - transform.position;
            Instantiate(bullet, shootPos.position, Quaternion.LookRotation(playerDirection)); // Spawns the bullet.
            bullet.GetComponent<Bullet>().self = GetComponentInParent<CapsuleCollider>();
            isShooting = false;
        }
    }

    public void MeleeAttack()
    {
        isMelee = true;
        animator.SetTrigger("Melee");
        isMelee = false;
    }

    public void Summoning()
    {
        isSummoning = true;
        if (enemiesInScene < maxEnemiesSpawn)
        {
            Vector3 randomOffset = Random.insideUnitSphere * spawnRange;
            Vector3 spawnPOS = minibossPOS.position + randomOffset;
            randomOffset.y = minibossPOS.position.y;
            Instantiate(enemyToSummon, spawnPOS, Quaternion.identity);
            enemiesInScene++;
        }
        isSummoning = false;
    }

    public void MinionDeath()
    {
        if (enemiesInScene > 0)
        {
            enemiesInScene--;
        }
    }

    // Method called when the enemy takes damage.
    public void takeDamage(int amount, int type)
    {
        HP -= amount; // Reduces health by the damage amount.
        StartCoroutine(FlashRed()); // Flashes red to indicate damaged.
        animator.SetTrigger("takeDamage");
        // Directs the enemy to move towards the player's position upon taking damage.
        agent.SetDestination(gameManager.instance.player.transform.position);

        // Checks if health has dropped to 0 or below.
        if (HP <= 0)
        {
            if (itemToDrop != null)
            {
                itemToDrop.SetActive(true);
            }
            stoppingDist = 100;
            animator.SetBool("Death", true);
            StartCoroutine(DeathAnimate());
            hpBar.gameObject.SetActive(false);
            minibossName.gameObject.SetActive(false);
        }
    }

    IEnumerator DeathAnimate()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    // Coroutine to visually indicate damage by changing the enemy's color.
    IEnumerator FlashRed()
    {
        model.material = damageMat; // Changes the model's material to the damage indicator.
        yield return new WaitForSeconds(0.1f); // Waits briefly.
        model.material = originalMat; // Restores the original material.
    }

    public void WeaponOn()
    {
        weaponCollider.enabled = true;
    }

    public void WeaponOff()
    {
        weaponCollider.enabled = false;
    }
    // Detects when the player enters the enemy's detection range.
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Marks that the player is in range.
            hpBar.gameObject.SetActive(true);
            minibossName.gameObject.SetActive(true);
            rand = new();
            action = rand.Next(4);
            MinibossAttack(action);
        }
    }

    // Detects when the player exits the enemy's detection range.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Marks that the player is no longer in range.
            agent.stoppingDistance = 0; // Resets the stopping distance.
            hpBar.gameObject.SetActive(false);
            minibossName.gameObject.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy AI behavior, including movement, shooting, and interactions with the player.
public class enemyAI : MonoBehaviour, IDamage, ISlow, IParalyze, IBurn
{
    [HeaderAttribute("-----Components-----")]
    [SerializeField] Renderer model; // The enemy's visual model.
    [SerializeField] NavMeshAgent agent; // Navigation component for AI movement.
    [SerializeField] Transform shootPos; // Position from which the enemy shoots.
    [SerializeField] GameObject itemToDrop; //Optional. Drops an item from a pre-defined position after enemy dies

    [HeaderAttribute("-----Enemy Stats-----")]
    [SerializeField] int HP; // Enemy health points.
    [SerializeField] int speed; // Movement speed.

    [HeaderAttribute("-----Shooting Stats-----")]
    [SerializeField] GameObject bullet; // Bullet prefab for shooting.
    [SerializeField] Material damageMat; // Material to indicate the enemy has taken damage.
    [SerializeField] float shootRate; // Rate at which the enemy shoots.
    [SerializeField] int stoppingDist; // Minimum distance to stop from the player.
    [SerializeField] float shootAniDelay; // Delay for the bullet based the enemy animation.
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

    [HeaderAttribute("-----The Stuffs-----")] //Joseph's section, plz no comments ;-;
    [SerializeField] int scoreValue;
    [SerializeField] int enemyType;//1 = water, 2 = fire, 3 = lightning, 4 = plant
    [SerializeField] GameObject[] drops;
    [SerializeField] int dropRolls;
    [SerializeField] Material paralysisMat;
    [SerializeField] GameObject rootEffect;
    [SerializeField] ParticleSystem particleCrit;
    [SerializeField] ParticleSystem particleWeak;
    [SerializeField] ParticleSystem particleNormal;
    ParticleSystem activeParticle;
    bool paralyzed;
    bool burning;
    bool dotCD;
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
        activeParticle = particleNormal;
    }

    void Update()
    {
        
        if (paralyzed && model.material == originalMat)
            model.material = paralysisMat;
        else if (!paralyzed)
        {
            //animator.SetBool("Aggro", true);
            float animSpeed = agent.velocity.normalized.magnitude; // Calculates speed for animation.
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans)); // Smoothly transitions animation speed.

            // Determines behavior based on player visibility and range.
            if (playerInRange && !canSeePlayer())
            {
                StartCoroutine(roam()); // Starts roaming if player is out of sight but in range.
                //animator.SetBool("Aggro", false);
            }
            else if (!playerInRange)
            {
                StartCoroutine(roam()); // Starts roaming if player is not in range.
                //animator.SetBool("Aggro", false);
            }
        }
        if (burning && !dotCD)
            StartCoroutine(BurnTick());
    }

    // Coroutine for roaming when the player is not detected.
    IEnumerator roam()
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

    // Coroutine for shooting at the player.
    IEnumerator shoot()
    {
        isShooting = true;
        bullet.GetComponent<Bullet>().self = GetComponentInParent<CapsuleCollider>();
        animator.SetTrigger("Shoot"); // Triggers the shooting animation.
        yield return new WaitForSeconds(shootAniDelay);
        
        Vector3 playerDirection = gameManager.instance.player.transform.position - transform.position;
        GameObject projectile = Instantiate(bullet, shootPos.position, Quaternion.LookRotation(playerDirection));

        yield return new WaitForSeconds(shootRate - shootAniDelay);
        yield return new WaitForSeconds(shootRate); // Waits before allowing next shot.
        isShooting = false;
    }

    public void takeDamage(int amount, int type)
    {
        Debug.Log("TakeDamageStart");
        //1 = water, 2 = fire, 3 = lightning, 4 = plant
        switch (enemyType)
        {
            case 1:
                switch (type)
                {
                    case 1:
                        HP -= amount;
                        activeParticle = particleNormal;
                        break;
                    case 2:
                        HP -= amount / 2;
                        activeParticle = particleWeak;
                        break;
                    case 3:
                        HP -= amount * 2;
                        activeParticle = particleCrit;
                        break;
                    case 4:
                        HP -= amount;
                        activeParticle = particleNormal;
                        break;
                }
                break;
            case 2:
                switch (type)
                {
                    case 1:
                        HP -= amount * 2;
                        activeParticle = particleCrit;
                        break;
                    case 2:
                        HP -= amount;
                        activeParticle = particleNormal;
                        break;
                    case 3:
                        HP -= amount;
                        activeParticle = particleNormal;
                        break;
                    case 4:
                        HP -= amount / 2;
                        activeParticle = particleWeak;
                        break;
                }
                break;
            case 3:
                switch (type)
                {
                    case 1:
                        HP -= amount / 2;
                        activeParticle = particleWeak;
                        break;
                    case 2:
                        HP -= amount;
                        activeParticle = particleNormal;
                        break;
                    case 3:
                        HP -= amount;
                        activeParticle = particleNormal;
                        break;
                    case 4:
                        HP -= amount * 2;
                        activeParticle = particleCrit;
                        break;
                }
                break;
            case 4:
                switch (type)
                {
                    case 1:
                        HP -= amount;
                        activeParticle = particleCrit;
                        break;
                    case 2:
                        HP -= amount * 2;
                        activeParticle = particleCrit;
                        break;
                    case 3:
                        HP -= amount / 2;
                        activeParticle = particleCrit;
                        break;
                    case 4:
                        HP -= amount;
                        activeParticle = particleCrit;
                        break;
                }
                break;
            default:
                HP -= amount;
                break;
        }
        activeParticle.Play();
        animator.SetTrigger("TakesDamage");

        StartCoroutine(flashRed());
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            agent.acceleration = 0;
            agent.velocity = Vector3.zero;
            if (associatedSpawner)
                associatedSpawner.UpdateEnemies(-1);
            gameManager.instance.playerScore += scoreValue;
            if (itemToDrop != null)
            {
                itemToDrop.SetActive(true);
            }
            RollForDrops();

            MinibossAI minibossAI = FindObjectOfType<MinibossAI>();
            if (minibossAI != null)
            {
                minibossAI.MinionDeath();
            }
            animator.SetBool("Dead", true);
            StartCoroutine(enemyDeath());
        }
    }

    IEnumerator enemyDeath()
    {
        yield return new WaitForSeconds(8);
            Destroy(gameObject);
        }

    public void EnemiesCelebrate()
    {
        if (!animator.GetBool("Dead"))  // Ensure dead enemies do not celebrate
        {
            animator.SetTrigger("PlayerIsDead");
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






    // JOSEPH'S SECTION \/ \/ \/ \/ PLZ DO NOT COMMENT BELOW THIS LINE I BEG OF THEE 
    public void GetSlowed(float slowModifier, int slowLength)
    {
        StartCoroutine(Slow(slowModifier, slowLength));
    }
    public void GetParalyzed(float duration)
    {
        StartCoroutine(Paralyzed(duration));
    }
    public void GetBurnt(int duration)
    {
        StartCoroutine(Burn(duration));
    }
    public void RollForDrops()
    {
        for (int i = 0; i < dropRolls; i++)
        {
            int arrayPOS = Random.Range(0, drops.Length);
            Instantiate(drops[arrayPOS], transform.position, transform.rotation);
        }
    }
    IEnumerator Slow(float slowMod, int slowLength)
    {
        agent.speed = originalSpeed * slowMod;
        rootEffect.SetActive(true);
        yield return new WaitForSeconds(slowLength);
        rootEffect.SetActive(false);
        agent.speed = originalSpeed;
    }
    IEnumerator Paralyzed(float duration)
    {
        paralyzed = true;
        yield return new WaitForSeconds(duration);
        model.material = originalMat;
        paralyzed = false;
    }
    IEnumerator Burn(int duration)
    {
        burning = true;
        yield return new WaitForSeconds(duration);
        burning = false;
    }
    IEnumerator BurnTick()
    {
        dotCD = true;
        yield return new WaitForSeconds(.4f);
        takeDamage(2, 2);
        dotCD = false;
    }
}

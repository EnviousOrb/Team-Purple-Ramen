using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Controls the player's movements, interactions, and inventory management.
public class playerController : MonoBehaviour, IDamage
{
    [HeaderAttribute("----- Components -----")]
    [SerializeField] CharacterController controller; // The CharacterController component for moving the player.
    [SerializeField] weaponController weapon; // The current weapon controller.
    [SerializeField] AudioSource AS; //The Audio Source component for the player

    [HeaderAttribute("----- Player Stats -----")]
    [Range(0, 10)][SerializeField] int HP; // The player's health points.
    [Range(1, 5)][SerializeField] float speed; // Movement speed of the player.
    [Range(2, 8)][SerializeField] float sprintMultiplier; // The multiplier to apply to speed when sprinting.
    [Range(1, 3)][SerializeField] int jumps; // The number of consecutive jumps the player can perform.
    [Range(5, 25)][SerializeField] int jumpSpeed; // The vertical speed of the player's jump.
    [Range(-15, -35)][SerializeField] int gravity; // The gravity affecting the player.
    

    [HeaderAttribute("----- Item Inventory -----")]
     public List<ItemData> itemList = new List<ItemData>(); // Player's inventory

    [HeaderAttribute("----- Weapon Components -----")]
    [SerializeField] Transform shootPos; // The position from which bullets are fired.
    [SerializeField] GameObject bullet; // The bullet prefab.

    [HeaderAttribute("----- Wizard Staff -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;

    [HeaderAttribute("----- Audio Components -----")]
    [SerializeField] AudioClip[] playerJump;
    [Range(0, 1)][SerializeField] float playerJumpVol;
    [SerializeField] AudioClip[] playerHurt;
    [Range(0, 1)][SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] playerSteps;
    [Range(0, 1)][SerializeField] float playerStepsVol;

    // Private variables for internal state management
    int jumpcount; // Tracks the number of jumps performed consecutively.
    Vector3 moveDir; // The direction of movement.
    Vector3 playerVel; // The player's current velocity.
    float originalSpeed; // The original speed of the player, for restoring after sprinting.
    int HPoriginal; // The original health points of the player, for UI updates.
    int selectedItem; // The index of the currently selected item.
    int rayDistance; // Distance for the raycast debug line.

    // bools
    bool isShooting; // Flag to indicate if the player is currently shooting.
    bool isMeleeing; // Flag to indicate if the player is currently meleeing.
    bool isSprinting; //Flag to indicate that the player is currently sprinting.
    bool playSteps;


    void Start()
    {
        originalSpeed = speed; // Store the original speed.
        HPoriginal = HP; // Store the original HP for UI calculations.
        updatePlayerUI(); // Update the UI elements based on current stats.
    }

    void Update()
    {
        if (!gameManager.instance.isPaused) // Check if the game is not paused.
        {
            //selectItem(); // Handle item selection.
#if UNITY_EDITOR
            // Draw a debug ray in the editor to visualize aiming or looking direction.
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.green);
#endif
            movement(); // Handle player movement.

            //changeWeapon(); // Handle weapon switching.
            // Check for shooting input and current weapon type to trigger appropriate attack.
            if (Input.GetButton("Shoot") && !isShooting && !isMeleeing)
            {
                StartCoroutine(shoot());
            }

            if (Input.GetButton("Fire2") && !isMeleeing && !isShooting)
            {
                StartCoroutine(melee());
            }
        }
    }

    public void spawnPlayer()
    {
        HP = HPoriginal; // Reset player HP to original value.
        updatePlayerUI(); // Update the player's UI elements.
        controller.enabled = false; // Temporarily disable the controller to move the player.
        transform.position = gameManager.instance.playerSpawnPos.transform.position; // Move player to spawn position.
        controller.enabled = true; // Re-enable the controller.
    }

    void movement()
    {
        // Handle sprinting input and adjust speed accordingly.
        if (Input.GetButton("Fire3"))
        {
            speed = originalSpeed * sprintMultiplier;
            isSprinting = true;

        }
        else
        {
            speed = originalSpeed;
            isSprinting = false;
        }
        // Reset jump count and player velocity when grounded.
        if (controller.isGrounded)
        {
            jumpcount = 0;
            playerVel = Vector3.zero;
        }

        // Calculate movement direction based on input.
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        // Apply movement.
        controller.Move(moveDir * speed * Time.deltaTime);

        // Handle jumping logic.
        if (Input.GetButtonDown("Jump") && jumpcount < jumps)
        {
            jumpcount++;
            playerVel.y = jumpSpeed;
            AS.PlayOneShot(playerJump[Random.Range(0, playerJump.Length)], playerJumpVol);
        }

        // Crouch functionality - reduces player height.
        if (Input.GetButtonDown("Crouch"))
        {
            crouch();
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            unCrouch();
        }

        // Apply gravity to the player's velocity and move the character controller accordingly.
        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (controller.isGrounded && moveDir.normalized.magnitude > 0.3f && !playSteps)
        {
            StartCoroutine(PlaySteps());
        }
    }

    IEnumerator PlaySteps()
    {
        playSteps = true;
        AS.PlayOneShot(playerSteps[Random.Range(0, playerSteps.Length)], playerStepsVol);

        if (!isSprinting)
            yield return new WaitForSeconds(0.3f);
        else
            yield return new WaitForSeconds(0.3f);

        playSteps = false;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        RaycastHit hit;
        Instantiate(bullet, shootPos.position, transform.rotation);
        
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            Debug.Log(hit.collider.name);

            IDamage damageTarget = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && damageTarget != null)
            {
                damageTarget.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator melee()
    {
        isMeleeing = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            Debug.Log(hit.collider.name);

        }
        yield return new WaitForSeconds(shootRate);
        isMeleeing = false;
    }

    // Implements IDamage interface's takeDamage method.
    public void takeDamage(int amount)
    {
        HP -= amount; // Decrease player's health by the damage amount.
        StartCoroutine(flashdmgScreen()); // Flash damage effect on screen.
        updatePlayerUI(); // Update player's health UI.

        AS.PlayOneShot(playerHurt[Random.Range(0, playerHurt.Length)], playerHurtVol);

        // Check for player death.
        if (HP <= 0)
        {
            gameManager.instance.stateLose(); // Trigger game loss state.
        }
    }

    // Coroutine to flash the damage screen effect.
    IEnumerator flashdmgScreen()
    {
        gameManager.instance.playerDamageEffect.SetActive(true); // Show damage effect.
        yield return new WaitForSeconds(.1f); // Wait for a brief moment.
        gameManager.instance.playerDamageEffect.SetActive(false); // Hide damage effect.
    }

    // Updates player's health bar UI.
    void updatePlayerUI()
    {
        gameManager.instance.HPbar.fillAmount = (float)HP / HPoriginal; // Set health bar based on current health.
    }

    // Reduces the player's height for crouching.
    void crouch()
    {
        controller.height /= 2;
    }

    // Resets the player's height after crouching.
    void unCrouch()
    {
        controller.height *= 2;
    }

    public void GetItem(ItemData newItem)
    {
        // Add the new item to the inventory
        itemList.Add(newItem);

        // Refresh the UI to reflect the new inventory state
        UIManager.instance.UpdateInventoryUI(itemList);
    }
}   

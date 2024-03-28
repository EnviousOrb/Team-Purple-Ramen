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
    [SerializeField] Animator anim;

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
    [SerializeField] Collider staffCollider;

    [HeaderAttribute("----- Wizard Range Attack -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;

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
    bool isCrouching;
    bool playSteps;
    bool isMoving;

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
            //if (Input.GetButton("Shoot") && !isShooting && !isMeleeing)
            if (Input.GetButton("Fire1") && !isShooting)
            {
                StartCoroutine(shoot());
            }

            if (Input.GetButton("Fire2") && !isMeleeing)
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

        // Animating idle to walk to run to sprint.
        // This does work but best in 3rd person.
        //anim.SetFloat("Speed", moveDir.magnitude);
        //anim.SetBool("IsSprinting", isSprinting);

        if (Input.GetButtonDown("Jump") && jumpcount < jumps)
        {
            jumpcount++;
            playerVel.y = jumpSpeed;
            AudioManager.instance.playPlayerSFX("Jump SFX");
            // Future proofing 3d person jump
            //anim.SetTrigger("Jump");
        }

        // Crouch functionality - reduces player height.
        //if (Input.GetButtonDown("Crouch"))
        //{
        //    crouch();
        //    //anim.SetBool("IsCrouching", true);
        //}
        //else if (Input.GetButtonUp("Crouch"))
        //{
        //    unCrouch();
        //    //anim.SetBool("IsCrouching", false);
        //}

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
        string[] footstepSounds = new string[]
        {
        "Footstep", "Footstep 1", "Footstep 2", "Footstep 3", "Footstep 4",
        "Footstep 5", "Footstep 6", "Footstep 7", "Footstep 8", "Footstep 9",
        "Footstep 10", "Footstep 11", "Footstep 12", "Footstep 13", "Footstep 14",
        "Footstep 15", "Footstep 16", "Footstep 17", "Footstep 18", "Footstep 19",
        "Footstep 20"
        };

        foreach (string footstepSound in footstepSounds)
        {
            if(moveDir.normalized.magnitude > 0.3f)
            {
                AudioManager.instance.playPlayerSFX(footstepSound);
            }
            else
            {
                AudioManager.instance.PlayerSource.Stop();
            }
            if (!isSprinting)
                yield return new WaitForSeconds(.8f);
            else
                yield return new WaitForSeconds(0.3f);
        }
        playSteps = false;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Casting");
        
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void FireBullet()
    {
        Instantiate(bullet, shootPos.position, Camera.main.transform.rotation);
    }

    public void startMeleeSwing() 
    {
        staffCollider.enabled = true;
    }

    public void exitMeleeSwing()
    {
        staffCollider.enabled = false;
    }

    IEnumerator melee()
    {
        isMeleeing = true;
        anim.SetTrigger("Melee");
        RaycastHit hit;
        //if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        //{
        //    Debug.Log(hit.collider.name);
        //}
        yield return new WaitForSeconds(shootRate);
        isMeleeing = false;
    }

    // Implements IDamage interface's takeDamage method.
    public void takeDamage(int amount)
    {
        HP -= amount; // Decrease player's health by the damage amount.
        StartCoroutine(flashdmgScreen()); // Flash damage effect on screen.
        updatePlayerUI(); // Update player's health UI.

        AudioManager.instance.playPlayerSFX("Damage SFX");

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

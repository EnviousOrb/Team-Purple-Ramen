using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage
{
    [HeaderAttribute("-----Components-----")]
    [SerializeField] CharacterController controller;
    [SerializeField] weaponController weapon;

    [HeaderAttribute("-----Player Stats-----")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 5)][SerializeField] float speed;
    [Range(1, 3)][SerializeField] int jumps;
    [Range(5, 25)][SerializeField] int jumpSpeed;
    [Range(-15, -35)][SerializeField] int gravity;
    [SerializeField] float sprintMultiplier;

    [HeaderAttribute("-----Item Inventory-----")]
    [SerializeField] public List<ItemData> ItemList = new List<ItemData>();
    [SerializeField] public List<Image> inventorySlotImage = new List<Image>();
    [SerializeField] public List<Image> inventoryBackgroundImage = new List<Image>();

    [HeaderAttribute("-----Weapon Inventory-----")]
    [SerializeField] public WeaponData[] WeaponList;


    int jumpcount;
    Vector3 moveDir;
    Vector3 playerVel;
    float originalSpeed;
    bool isShooting;
    int HPoriginal;
    int selectedItem;
    int currentWeaponIndex;
    int rayDistance;

    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;
        HPoriginal = HP;
        updatePlayerUI();

        if (WeaponList.Length > 0)
        {
            weapon = WeaponList[0].weaponModel.GetComponent<weaponController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            selectItem();
#if UNITY_EDITOR
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.green);
#endif
            movement();

            changeWeapon();
            if (Input.GetButton("Shoot") && !isShooting)
            {
                if (WeaponList[currentWeaponIndex].Tag == "Melee")
                {
                    StartCoroutine(weapon.SwordAttack());
                }
                else if (WeaponList[currentWeaponIndex].Tag == "Ranged")
                {
                    StartCoroutine(weapon.RangedAttack());
                }
            }
        }
    }
    public void spawnPlayer()
    {
        HP = HPoriginal;
        updatePlayerUI();
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }
    void movement()
    {
        if (Input.GetButton("Fire3"))
            speed = originalSpeed * sprintMultiplier;
        else
            speed = originalSpeed;

        if (controller.isGrounded)
        {
            jumpcount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpcount < jumps)
        {
            jumpcount++;
            playerVel.y = jumpSpeed;
        }

        if(Input.GetButtonDown("Crouch"))
        {
            crouch();
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            unCrouch();
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashdmgScreen());
        updatePlayerUI();

        if (HP <= 0)
        {
            gameManager.instance.stateLose();
        }
    }

    IEnumerator flashdmgScreen()
    {
        gameManager.instance.playerDamageEffect.SetActive(true);
        yield return new WaitForSeconds(.1f);
        gameManager.instance.playerDamageEffect.SetActive(false);
    }

    void updatePlayerUI()
    {
        gameManager.instance.HPbar.fillAmount = (float)HP / HPoriginal;
    }

    void crouch()
    {
        controller.height /= 2;

    }

    void unCrouch()
    {
        controller.height *= 2;
    }

    public void GetItem(ItemData Item)
    {
        ItemList.Add(Item);
        // Update the inventory slot images
        for (int i = 0; i < inventorySlotImage.Count; i++)
        {
            if (i < ItemList.Count)
            {
                inventorySlotImage[i].sprite = ItemList[i].itemSprite;
            }
            else
            {
                inventorySlotImage[i].sprite = null;
            }
        }
    }

    void selectItem()
    {
        for (int i = 0; i < 7; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && ItemList.Count > i)
            {
                selectedItem = i;
                break;
            }
        }
    }

    void changeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % WeaponList.Length;
            weapon = WeaponList[currentWeaponIndex].weaponModel.GetComponent<weaponController>();
        }
    }
}

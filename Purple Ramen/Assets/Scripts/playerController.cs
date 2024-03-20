using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage
{
    [HeaderAttribute("-----Components-----")]
    [SerializeField] CharacterController controller;

    [HeaderAttribute("-----Player Stats-----")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 5)][SerializeField] float speed;
    [Range(1, 3)][SerializeField] int jumps;
    [Range(5, 25)][SerializeField] int jumpSpeed;
    [Range(-15, -35)][SerializeField] int gravity;
    [SerializeField] float sprintMultiplier;

    [HeaderAttribute("-----Gun Stats-----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;

    [HeaderAttribute("-----Item Inventory-----")]
    [SerializeField] public List<ItemData> ItemList = new List<ItemData>();
    [SerializeField] public List<Image> inventorySlotImage = new List<Image>();
    [SerializeField] public List<Image> inventoryBackgroundImage = new List<Image>();
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] GameObject ItemModel;

    int jumpcount;
    Vector3 moveDir;
    Vector3 playerVel;
    float originalSpeed;
    bool isShooting;
    int HPoriginal;
    int selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;
        HPoriginal = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            selectItem();
#if UNITY_EDITOR
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.green);
#endif
            movement();

            if (Input.GetButton("Shoot") && !isShooting)
            {
                StartCoroutine(shoot());
            }
        }
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

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
                inventorySlotImage[i].sprite = emptySlotSprite;
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
                changeItem();
                break;
            }
        }
    }

    void changeItem()
    {
        if (selectedItem >= 0 && selectedItem < ItemList.Count)
        {
            ItemModel = ItemList[selectedItem].model;
            Debug.Log("Item changed to " + ItemModel);
        }
    }
}

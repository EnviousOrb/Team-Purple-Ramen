using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    int maxhp;
    bool isShooting;
    int jumpcount;
    Vector3 moveDir;
    Vector3 playerVel;
    float originalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;
        maxhp = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
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
        if(HP <= 0) 
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


}

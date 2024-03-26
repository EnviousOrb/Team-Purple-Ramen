using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuWin;
    [SerializeField] public GameObject catMenu;
    [SerializeField] public GameObject cauldronMenuGood;
    [SerializeField] public GameObject cauldronMenuBad;
    [SerializeField] public GameObject beanBox;
    [SerializeField] public GameObject beefBox;
    [SerializeField] public GameObject blueCheeseBox;
    [SerializeField] public GameObject butterBox;
    [SerializeField] public GameObject carrotBox;
    [SerializeField] public GameObject cheeseBox;
    [SerializeField] public GameObject chickenBox;
    [SerializeField] public GameObject clamsBox;
    [SerializeField] public GameObject crabBox;
    [SerializeField] public GameObject eggsBox  ;
    [SerializeField] public GameObject fishBox;
    [SerializeField] public GameObject flourBox ;
    [SerializeField] public GameObject garlicBox;
    [SerializeField] public GameObject honeyBox ;
    [SerializeField] public GameObject lobsterBox;
    [SerializeField] public GameObject octopusBox;
    [SerializeField] public GameObject oliveOilBox;
    [SerializeField] public GameObject onionsBox;
    [SerializeField] public GameObject oystersBox;
    [SerializeField] public GameObject parsnipsBox;
    [SerializeField] public GameObject potatoesBox;
    [SerializeField] public GameObject riceBox;
    [SerializeField] public GameObject saltBox;
    [SerializeField] public GameObject shrimpBox;
    [SerializeField] public GameObject sugarBox;
    [SerializeField] public GameObject turnipBox;
    [SerializeField] public GameObject vinegarBox;
    [SerializeField] public GameObject whiteCheddarBox;
    public GameObject playerDamageEffect;
    public Image HPbar;
    [SerializeField] TMP_Text countText;

    public GameObject playerSpawnPos;
    public Image playerHPBar;
    public GameObject player;
    public playerController PS;

    public bool isPaused;
    float TimeScaleOrig;
    int enemyCount;

    // Awake is called before Start
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        PS = player.GetComponent<playerController>();
        TimeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
                statePaused();
            else if (menuActive == menuPause)
                stateNormal();
        }

    }
    public void stateWin()
    {
        menuActive = menuWin;
        Pause();
    }
    public void statePaused()
    {
        menuActive = menuPause;
        Pause();
    }
    public void stateLose()
    {
        menuActive = menuLose;
        Pause();
    }
    public void stateNormal()
    {
        isPaused = false;
        Time.timeScale = TimeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menuActive.SetActive(true);
    }
}
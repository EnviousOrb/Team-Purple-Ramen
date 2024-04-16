using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STMTools;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuInv;
    [SerializeField] public GameObject TextBox;
    public GameObject checkpointMenu;
    public GameObject playerDamageEffect;
    public GameObject playerSlowEffect;
    public Image HPbar;
    [SerializeField] SuperTextMesh TextBoxText;

    public GameObject playerSpawnPos;
    public Image playerHPBar;
    public GameObject player;
    public playerController PS;
    public int playerScore;

    public bool isPaused;
    float TimeScaleOrig;

    // Awake is called before Start
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        PS = player.GetComponent<playerController>();
        TimeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        DontDestroyOnLoad(gameObject);
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            if(menuActive == null)
                stateInv();
            else if (menuActive == menuInv)
                stateNormal();
        }
    }

    public void UpdateTextBox(string newText)
    {
        if (TextBoxText.reading == true)
        {
            TextBoxText.Rebuild();
        }
        TextBox.SetActive(true);
        TextBoxText.text = newText;
    }
    public void HideTextBox()
    {
         TextBox.SetActive(false);
    }

    public void stateWin()
    {
        menuActive = menuWin;
        HideTextBox();
        Pause();
        AudioManager.instance.stopAll();
        AudioManager.instance.playSFX("Level Complete");
        AudioManager.instance.stopAll();
    }
    public void stateInv()
    {
        menuActive = menuInv;
        HideTextBox();
        Pause();
    }
    public void statePaused()
    {
        menuActive = menuPause;
        HideTextBox();
        Pause();
    }
    public void stateLose()
    {
        menuActive = menuLose;
        HideTextBox();
        Pause();
        AudioManager.instance.stopAll();
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
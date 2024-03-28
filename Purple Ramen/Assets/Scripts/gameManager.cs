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
    [SerializeField] public GameObject TextBox;
    public GameObject playerDamageEffect;
    public Image HPbar;
    [SerializeField] TMP_Text TextBoxText;

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

    public void UpdateTextBox(string newText)
    {
        TextBoxText.text = newText;
        TextBox.SetActive(true);
    }
    public void HideTextBox()
    {
        TextBox.SetActive(false);
    }

    public void stateWin()
    {
        menuActive = menuWin;
        Pause();
        AudioManager.instance.stopAll();
        AudioManager.instance.playSFX("Level Complete");
        AudioManager.instance.stopAll();
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
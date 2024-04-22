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
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuInv;
    [SerializeField] public GameObject menuSettings;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] public GameObject TextBox;
    public GameObject checkpointMenu;
    public GameObject playerDamageEffect;
    public GameObject playerSlowEffect;
    public GameObject playerHealEffect;
    public GameObject playerManaEffect;
    public Image HPbar;
    [SerializeField] public SceneInfo sceneInfo;
    [SerializeField] SuperTextMesh TextBoxText;

    public GameObject playerSpawnPos;
    public GameObject player;
    public playerController PS;
    public int playerScore;

    public bool isPaused;
    float TimeScaleOrig;

    private GameObject previousMenu;

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

    private void Start()
    {
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
            else if (menuActive == menuMain)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (menuActive == null)
                stateInv();
            else if (menuActive == menuInv)
                stateNormal();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIManager.instance.hotbarWeapon.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            UIManager.instance.hotbarWeapon.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PS.crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            PS.unCrouch();
        }
    }

    public void UpdateTextBox(string newText)
    {
        StopCoroutine(SuperHideTextBox(20));
        if (TextBoxText.reading == true)
        {
            TextBoxText.Rebuild();
        }
        TextBox.SetActive(true);
        TextBoxText.text = newText;
        StartCoroutine(SuperHideTextBox(20));
    }

    public void HideTextBox()
    {
        TextBox.SetActive(false);
    }

    public IEnumerator SuperHideTextBox(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideTextBox();
    }

    public void stateWin()
    {
        menuActive = menuWin;
        HideTextBox();
        Pause();
        AudioManager.instance.stopAll();
        AudioManager.instance.playSFX(AudioManager.instance.SFX[0].soundName);
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

    public void stateSettings()
    {
        previousMenu = menuActive;
        menuActive = menuSettings;
        menuSettings.SetActive(true);
        SetCursorState(true, CursorLockMode.None);
        HideTextBox();
    }


    public void stateLose()
    {
        menuActive = menuLose;
        HideTextBox();
        Pause();
        NotifyEnemiesPlayerDied();
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
        previousMenu = null;
    }

    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menuActive.SetActive(true);
    }

    public void SetCursorState(bool visible, CursorLockMode lockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = lockMode;
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        menuActive = menuMain;
        Pause();
    }

    public void NotifyEnemiesPlayerDied()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<enemyAI>()?.EnemiesCelebrate();
        }
    }
}
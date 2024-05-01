using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using STMTools;
using JetBrains.Annotations;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuInv;
    [SerializeField] GameObject menuSettings;
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

    //testing variable
    public bool playerDead;


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
            else if (menuActive == menuSettings)
            {
                menuActive = previousMenu;
                menuSettings.SetActive(false);
            }
            


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
        if (TextBoxText.reading == true)
        {
            TextBoxText.Rebuild();
        }
        TextBox.SetActive(true);
        TextBoxText.text = newText;
        SuperHideTextBox(8);
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
        HideTextBox();
        Pause();
    }
    public void stateLose()
    {
        menuActive = menuLose;
        HideTextBox();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerDead = true;
        menuActive.SetActive(true);
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
    }
    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menuActive.SetActive(true);
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

    public void resume()
    {
        gameManager.instance.stateNormal();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateNormal();
    }
    public void exitSettings()
    {
        menuActive = previousMenu;
        menuSettings.SetActive(false);
    }
    public void respawn()
    {
        gameManager.instance.stateNormal();
        gameManager.instance.PS.spawnPlayer();
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void showCredits()
    {
        gameManager.instance.ShowCredits();
    }
    public void settings()
    {
        gameManager.instance.stateSettings();
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuWin;
    [SerializeField] TMP_Text countText;
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
    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;
        countText.text = enemyCount.ToString();
        if (enemyCount == 0)
        {
            stateWin();
        }
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;

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
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
    }

    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateResume()
    {
        isPaused = !isPaused;
        Time.timeScale = TimeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }
    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        if (enemyCount <= 0)
        {
            statePaused();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }
    public void UpdateEnemyCount(int amount)
    {
        enemyCount -= amount;
    }

}
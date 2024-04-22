using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
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
        gameManager.instance.menuSettings.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnExitLevel : MonoBehaviour
{

    public string sceneName;
    public GameObject Loadingscreen;
    public int loadingTimer;
    public bool isNextScene = true;

    [SerializeField] public SceneInfo sceneInfo;

    private void Start()
    {
        StartCoroutine(Loading());
        gameManager.instance.PS.LoadPlayer();
        StopCoroutine(Loading());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.PS.SavePlayer();
            sceneInfo.isNextScene = isNextScene;
            StartCoroutine(Loading());
            SceneManager.LoadScene(sceneName);
            StopCoroutine(Loading());
        }
    }
    IEnumerator Loading()
    {
        Loadingscreen.SetActive(true);
        yield return new WaitForSeconds(loadingTimer);
        Loadingscreen.SetActive(false);
    }
}

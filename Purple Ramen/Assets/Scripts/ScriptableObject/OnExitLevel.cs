using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnExitLevel : MonoBehaviour
{

    public string sceneName;
    public bool isNextScene = true;

    [SerializeField] public SceneInfo sceneInfo;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            sceneInfo.isNextScene=isNextScene;
            SceneManager.LoadScene(sceneName);
        }
    }

}

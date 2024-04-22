using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName ="SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{

    public static SceneInfo instance;

   //public List<staffElementalStats> staffList;

    private void Awake()
    {

        instance = this;
        if (instance == null)
        {
            DontDestroyOnLoad(instance);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
        gameManager.instance.PS.LoadPlayer();
        
        //staffList= gameManager.instance.PS.staffList;
    }

  






    public bool isNextScene = true;
}




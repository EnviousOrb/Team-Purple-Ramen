using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName ="SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{

    public static SceneInfo instance;

    public int HP;
    public int HPorig;
    public float speed;


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
        

    }

   void  OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           

        }
       
    }






    public bool isNextScene = true;
}




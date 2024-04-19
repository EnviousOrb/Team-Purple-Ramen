using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName ="SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{

    public static SceneInfo Instance;

    public int HP;
    public int HPorig;
    public float speed;


    private void Awake()
    {

        Instance = this;
        if (Instance == null)
        {
            DontDestroyOnLoad(Instance);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
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




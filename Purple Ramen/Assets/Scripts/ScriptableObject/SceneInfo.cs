using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName ="SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{
    //SceneInfo functions like a Staff Manager
    public static SceneInfo instance;

    public List<staffElementalStats> staffList= new List<staffElementalStats>();
    public int selectedStaffIndex = 0;

    private void Awake()
    {

        //instance = this;
        //if (instance == null)
        //{
        //    DontDestroyOnLoad(instance);
        //    instance = this;
        //}
        //else if (instance != this)
        //{
        //    Destroy(instance);
        //}
        //gameManager.instance.PS.LoadPlayer();
        
        //staffList= gameManager.instance.PS.staffList;
    }

  

    public void AddStaff(staffElementalStats staff)
    {
        staffList.Add(staff);
    }


    

    public bool isNextScene = true;
}




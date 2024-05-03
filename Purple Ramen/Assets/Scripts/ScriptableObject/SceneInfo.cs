using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{
    //SceneInfo functions like a Staff Manager
    public static SceneInfo instance;
    public List<staffElementalStats> staffList = new();
    public int selectedStaffIndex = 0;

    public bool isNextScene = true;
}




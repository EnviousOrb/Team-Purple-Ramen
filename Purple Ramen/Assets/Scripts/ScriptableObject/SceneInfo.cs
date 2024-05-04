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
    public List<ItemData> nonPersistentItems = new();
    public int selectedStaffIndex = 0;

    public bool isNextScene = true;

    public bool isBGMSourceMuted;
    public bool isSFXSourceMuted;
    public bool isPlayerSourceMuted;
    public bool isBossSourceMuted;
    public bool isEnemySourceMuted;
    public bool isNPCSourceMuted;

    public float BGMSourceVolume;
    public float SFXSourceVolume;
    public float PlayerSourceVolume;
    public float BossSourceVolume;
    public float EnemySourceVolume;
    public float NPCSourceVolume;
}




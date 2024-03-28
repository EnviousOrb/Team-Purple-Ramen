using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponData : ScriptableObject
{
    public int Damage;
    public int Range;
    public float cooldown;
    public AudioClip AC;
    public GameObject weaponModel;
    public string Tag;
}

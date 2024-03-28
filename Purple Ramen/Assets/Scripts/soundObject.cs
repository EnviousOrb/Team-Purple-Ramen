using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Objects/Sound")]

public class soundObject : ScriptableObject
{
    public string soundName;
    public AudioClip soundClip;
}

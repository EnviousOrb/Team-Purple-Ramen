using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerState : MonoBehaviour
{

    public Serializables localPlayerData = new Serializables();

    void Start()
    {

    }
    public void SavePlayer()
    {
        GlobalControl.Instance.savedPlayerData = localPlayerData;
    }
}

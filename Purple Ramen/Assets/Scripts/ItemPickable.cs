using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour,Ipickable
{
    public ItemData itemScriptableObject;
    public void PickItem()
    {
        Destroy(gameObject);
    }
}

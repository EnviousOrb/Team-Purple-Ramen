using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    [Header("Properties")]
    public GameObject model;
    public Sprite itemSprite;

    public ItemData Clone()
    {
        ItemData clone = ScriptableObject.CreateInstance<ItemData>();
        clone.model = this.model;
        clone.itemSprite = this.itemSprite;
        return clone;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    [Header("Properties")]
    public itemType itemType;
    public Sprite itemSprite;
}

public enum itemType {Apple, Banana, Carrot, Fish, Mushroom, Tomato, Watermelon};

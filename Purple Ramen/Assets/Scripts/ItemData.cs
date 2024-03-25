using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName; // Optional, for identification
    public Sprite itemSprite; // The sprite to display in the UI
}


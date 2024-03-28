using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class recipeManager
{
    public List<ItemData> requiredItems; // Items needed for the recipe
    public ItemData resultItem; // Item produced by the recipe
}

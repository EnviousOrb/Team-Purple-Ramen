using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
// This code is being used as a weapon stats code.                          //
// This attaches to each staff (orb in this case) giving each unique stats. // 
//////////////////////////////////////////////////////////////////////////////

// Creats a quick create under the drop down menu in Unity.
// Will be under Asset > Create > Scriptable Object > Staff and will name the object Element Spell Stats.
[CreateAssetMenu(fileName = "Element Spell Stats", menuName = "Scriptable Objects/Staff")]
public class staffElementalStats : ScriptableObject, IInventory
{
    public Sprite InventorySprite
    {
        get { return itemSprite; }
        set { itemSprite = value; }
    }

    public string InventoryText
    {
        get { return itemDescription; }
        set { itemDescription = value; }
    }

    // Stats will be editable like a serialized field
    public int spellDamage;
    public int spellRange;
    public float spellCastRate;

    public string itemName; 
    public Sprite itemSprite; 
    public string itemDescription;

    // Objects to attach to staff
    public GameObject staffOrbModelPrefab;
    public GameObject projectilePrefab;

    public ParticleSystem onHitEffect;
    public ParticleSystem onCastEffect;
    public AudioClip spellSound;
    [Range(0,1)]public float shootSoundVol;
    
}

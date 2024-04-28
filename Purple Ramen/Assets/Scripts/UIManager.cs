using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dictionary<int, IInventory> reservedSpots4Staffs = new();
    [SerializeField] public List<Image> inventoryUISlotLocation = new(); // UI images for inventory slots.
    [SerializeField] public List<Image> weaponHotbar = new();
    [SerializeField] public GameObject hotbarWeapon;
    [SerializeField] public staffElementalStats weapon;
    [SerializeField] private Image mainSlotImage; // UI image for the main slot.
    [SerializeField] private SuperTextMesh descriptionText;
    public Slider bgmSlider, sfxSlider, npcSlider;

    public static UIManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        UpdateInventoryUI(weapon);
    }

    public void UpdateMainSlot(IInventory item)
    {
        mainSlotImage.sprite = item.InventorySprite;
        descriptionText.text = item.InventoryText;
    }
    public void UpdateWeaponHotbar(List<IInventory> weaponList)
    {
        foreach (var slot in weaponHotbar)
        {
            slot.sprite = null;
            slot.enabled = false;
        }

        List<IInventory> weaponItems = weaponList.Where(item => item is staffElementalStats).ToList();

        for(int i = 1; i < Math.Min(weaponItems.Count, weaponHotbar.Count); i++)
        {
            IInventory currentWeapon = weaponItems[i];

            weaponHotbar[i].sprite = currentWeapon.InventorySprite;
            weaponHotbar[i].enabled = true;
        }
    }

    public void UpdateInventoryUI(IInventory item)
    {
        //clears any previous items from the inventory UI (this will probably be used when transitioning from level to hub)
        foreach (var slot in inventoryUISlotLocation)
        {
            slot.sprite = null;
            slot.enabled = false;
        }

        if (item is staffElementalStats)
        {
            reservedSpots4Staffs[0] = item;

            inventoryUISlotLocation[0].sprite = item.InventorySprite;
            inventoryUISlotLocation[0].enabled = true;

            if (inventoryUISlotLocation[0].TryGetComponent<Button>(out var slotButton))
            {
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => UpdateMainSlot(item));
            }

            UpdateWeaponHotbar(new List<IInventory> { item });
        }
    }


    public void UpdateInventoryUI(List<IInventory> itemList)
    {
        foreach(var slot in inventoryUISlotLocation)
        {
            slot.sprite = null;
            slot.enabled = false;
        }

        List<IInventory> weaponItems = itemList.Where(item => item is staffElementalStats).ToList();
        List<IInventory> otherItems = itemList.Except(weaponItems).ToList();

        UpdateWeaponHotbar(weaponItems);

        int slotIndex = 0;

        //assign rest of items to generic slots
        foreach (IInventory item in otherItems)
        {
            if (slotIndex >= inventoryUISlotLocation.Count)
                break;

            //if it is, then go through motions
            inventoryUISlotLocation[slotIndex].sprite = item.InventorySprite;
            inventoryUISlotLocation[slotIndex].enabled = true;

            //update the onClick event like before
            if (inventoryUISlotLocation[slotIndex].TryGetComponent<Button>(out var slotButton))
            {
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => UpdateMainSlot(item));
            }
            slotIndex++;
        }
    }

    public void toggleBGM()
    {
        AudioManager.instance.toggleBGM();
    }

    public void toggleSFX()
    {
        AudioManager.instance.toggleSFX();
    }

    public void toggleNPC()
    {
        AudioManager.instance.toggleNPC();
    }

    public void BGMVolume()
    {
        AudioManager.instance.bgmVolume(bgmSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.bgmVolume(sfxSlider.value);
    }

    public void NPCVolume()
    {
        AudioManager.instance.NPCVolume(npcSlider.value);
    }
}


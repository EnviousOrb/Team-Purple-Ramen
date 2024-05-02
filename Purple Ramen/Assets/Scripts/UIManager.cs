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
    [SerializeField] private Image mainSlotImage; // UI image for the main slot.
    [SerializeField] private SuperTextMesh descriptionText;
    [SerializeField] SceneInfo sceneInfo;
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
        foreach(var staff in sceneInfo.staffList)
        {
            UpdateInventoryUI(staff);
        }
    }

    public void UpdateMainSlot(IInventory item)
    {
        mainSlotImage.sprite = item.InventorySprite;
        descriptionText.text = item.InventoryText;
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
        }
    }
    public void UpdateInventoryUI(List<IInventory> itemList)
    {
        int slotIndex = 0;
        //same as it ever was
        foreach (IInventory item in itemList)
        {
            if (slotIndex >= inventoryUISlotLocation.Count)
                break;

            //same as it ever was
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


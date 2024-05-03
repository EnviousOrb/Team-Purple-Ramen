using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
        UpdateInventoryUI(sceneInfo.staffList.Cast<IInventory>().ToList());
    }

    public void UpdateMainSlot(IInventory item)
    {
        mainSlotImage.sprite = item.InventorySprite;
        descriptionText.text = item.InventoryText;
    }

    public void UpdateInventoryUI(List<IInventory> itemList)
    {
        itemList = sceneInfo.staffList.Cast<IInventory>().ToList();

        for (int slotIndex = 0; slotIndex < inventoryUISlotLocation.Count; slotIndex++)
        {
            //clear other slots
            inventoryUISlotLocation[slotIndex].sprite = null;
            inventoryUISlotLocation[slotIndex].enabled = false;

            //if empty, update it
            if (slotIndex < itemList.Count)
            {
                //same as it ever was
                IInventory item = itemList[slotIndex];
                inventoryUISlotLocation[slotIndex].sprite = item.InventorySprite;
                inventoryUISlotLocation[slotIndex].enabled = true;

                if (inventoryUISlotLocation[slotIndex].TryGetComponent<Button>(out var slotButton))
                {
                    slotButton.onClick.RemoveAllListeners();
                    slotButton.onClick.AddListener(() => UpdateMainSlot(item));
                }
            }
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


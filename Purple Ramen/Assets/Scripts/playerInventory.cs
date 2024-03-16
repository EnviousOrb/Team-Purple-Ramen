using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class playerInventory : MonoBehaviour
{
    [Header("General")]
    public List<itemType> inventoryList;
    public int playerReach;
    [SerializeField] Camera cam;
    [SerializeField] GameObject throwItem;
    [SerializeField] GameObject pickUpItem;
    [SerializeField] TMPro.TextMeshProUGUI pickupText;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;

    public int selectedItem = 0;

    [Space(20)]
    [Header("Item GameObjects")]
    [SerializeField] GameObject item1;
    [SerializeField] GameObject item2;
    [SerializeField] GameObject item3;
    [SerializeField] GameObject item4;
    [SerializeField] GameObject item5;
    [SerializeField] GameObject item6;
    [SerializeField] GameObject item7;

    [Space(20)]
    [Header("Item Prefabs")]
    [SerializeField] GameObject item1Prefab;
    [SerializeField] GameObject item2Prefab;
    [SerializeField] GameObject item3Prefab;
    [SerializeField] GameObject item4Prefab;
    [SerializeField] GameObject item5Prefab;
    [SerializeField] GameObject item6Prefab;
    [SerializeField] GameObject item7Prefab;

    [Space(20)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[7];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[7];
    [SerializeField] Sprite emptySlotSprite;


    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };

    void Start()
    {
        itemSetActive.Add(itemType.Apple, item1);
        itemSetActive.Add(itemType.Banana, item2);
        itemSetActive.Add(itemType.Carrot, item3);
        itemSetActive.Add(itemType.Fish, item4);
        itemSetActive.Add(itemType.Mushroom, item5);
        itemSetActive.Add(itemType.Tomato, item6);
        itemSetActive.Add(itemType.Watermelon, item7);

        itemInstantiate.Add(itemType.Apple, item1Prefab);
        itemInstantiate.Add(itemType.Banana, item2Prefab);
        itemInstantiate.Add(itemType.Carrot, item3Prefab);
        itemInstantiate.Add(itemType.Fish, item4Prefab);
        itemInstantiate.Add(itemType.Mushroom, item5Prefab);
        itemInstantiate.Add(itemType.Tomato, item6Prefab);
        itemInstantiate.Add(itemType.Watermelon, item7Prefab);

        NewItemSelected();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo, playerReach))
        {
            Ipickable item = hitinfo.collider.GetComponent<Ipickable>();
            if (item != null)
            {
                pickUpItem.SetActive(true);
                pickupText.text = "Press 'E' to pick up";
                if (Input.GetKey(pickItemKey))
                {
                    inventoryList.Add(hitinfo.collider.GetComponent<ItemPickable>().itemScriptableObject.itemType);
                    item.PickItem();
                }
            }
            else
            {
                pickUpItem.SetActive(false);
                pickupText.text = "";
            }
        }
        else
        {
            pickUpItem.SetActive(true);
            pickupText.text = "";
        }
        //Item throw
        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 0)
        {
            Instantiate(itemInstantiate[inventoryList[selectedItem]], position: throwItem.transform.position, new Quaternion());
            inventoryList.RemoveAt(selectedItem);

            if (selectedItem != 0)
            {
                selectedItem -= 1;
            }
            NewItemSelected();
        }
        //UI
        for (int i = 0; i < inventorySlotImage.Length; i++)
        {
            if (i < inventoryList.Count)
            {
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemData.itemSprite;
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Count > 3)
        {
            selectedItem = 3;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && inventoryList.Count > 4)
        {
            selectedItem = 4;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && inventoryList.Count > 5)
        {
            selectedItem = 5;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && inventoryList.Count > 6)
        {
            selectedItem = 6;
            NewItemSelected();
        }
    }

    private void NewItemSelected()
    {
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);
        item4.SetActive(false);
        item5.SetActive(false);
        item6.SetActive(false);
        item7.SetActive(false);

        if (selectedItem >= 0 && selectedItem < inventoryList.Count)
        {
            GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
            selectedItemGameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Invalid item selection");
        }
    }
}

public interface Ipickable
{
    void PickItem();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryController : MonoBehaviour {
	GameObject inventoryPanel;
	GameObject slotPanel;
    GameObject player;
	ItemDatabase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;
    public GameObject inGameItem;
    public Tooltip tooltip;

    private RectTransform panelBounds;
	private int maxSlotCount;
    private int currentSlotCount;

	public List<Item> items = new List<Item>();
	public List<GameObject> slots = new List<GameObject>();
    
	void Start() {
        player = GameObject.Find("Player");
        database = GameObject.FindObjectOfType<ItemDatabase>();
		maxSlotCount = 20;
        currentSlotCount = 0;
		inventoryPanel = GameObject.Find("Inventory Panel");
        panelBounds = inventoryPanel.transform as RectTransform;
		slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;

        for (int i = 0; i < maxSlotCount; i++) {
			items.Add(new Item());
			slots.Add(Instantiate(inventorySlot));
			slots[i].GetComponent<InventorySlot>().id = i;
			slots[i].transform.SetParent(slotPanel.transform);
        }
        for (int i = 0; i < slots.Count; i++) {
            slots[i].name = "Empty Inventory Slot";
        }
        AddItem(new Item(), 10);
        AddItem(new Item(), 10);
        AddItem(new Item(), 11);
        Debug.Log("item in second slot: " + items[1].UniqueID);
		AddItem(new Item(), 11);
		AddItem(new Item(), 11);
        inventoryPanel.SetActive(false);
    }

	void Update() {
		if(Input.GetKeyDown(KeyCode.I)) {
			TogglePanel();
		}

	}

    public void TogglePanel()
    {
        if(inventoryPanel.activeSelf) {
                tooltip.Deactivate();
                inventoryPanel.SetActive(false);
			} else {
				inventoryPanel.SetActive(true);
			}
    }

	public void AddItem(Item oldItem, int id) {
        if (oldItem.UniqueID != Guid.Empty) {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == -1) {
                    currentSlotCount++;
                    items[i] = oldItem;

                    // Completely UI tweaks, no game functionality
                    
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<InventorySlotData>().item = oldItem;
                    itemObj.GetComponent<InventorySlotData>().slot = i;
                    itemObj.GetComponent<InventorySlotData>().inventoryPanelBounds = panelBounds;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = slots[i].transform.position;
                    if (items[i].Stackable) {
                        itemObj.GetComponent<InventorySlotData>().transform.GetChild(0).GetComponent<Text>().text = items[i].Quantity.ToString();
                    }
                    itemObj.GetComponent<Image>().sprite = oldItem.Sprite;
                    itemObj.name = oldItem.Title;
                    slots[i].name = oldItem.Title + " Slot";
                    break;
                }
            }
        } else {
		    Item templateItem = database.FetchItemByID(id);
            if (templateItem is Weapon) {
                Weapon weaponToAdd = new Weapon((Weapon)templateItem);
                addWeapon(id, weaponToAdd);
            }
        }
	}

    public void DropItem(Guid itemUniqueID) {
        for (int i = 0; i < items.Count; i++) {
            if (items[i].UniqueID == itemUniqueID ) {
                GameObject oldItem = Instantiate(inGameItem, new Vector3(player.transform.position.x, player.transform.position.y - 0.8f, 0), Quaternion.identity);
                oldItem.GetComponent<ItemBehavior>().inventoryController = this.GetComponent<InventoryController>();
                Debug.Log("This gets ran");
                oldItem.GetComponent<ItemBehavior>().setItemByUniqueID(items[i].UniqueID);
                items[i] = new Item();
                currentSlotCount--;

                // Completely UI tweaks, no game functionality;
                slots[i].name = "Empty Inventory Slot";
                Destroy(slots[i].transform.GetChild(0).gameObject);
                break;
            }
        }
    }

	bool IsItemInInventory(Item item) {
		for(int i = 0; i < items.Count; i++) {
			if(items[i].ID == item.ID) {
				return true;
			}
		}
		return false;
	}

    public bool IsInventoryFull() {
        if(currentSlotCount == maxSlotCount) {
            return true;
        } else {
            return false;
        }
    }

    public Item FetchItemByUniqueID(Guid uniqueID)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].UniqueID == uniqueID) {
                return items[i];
            }
        }
        return null;
    }
    
    // Add item functions: Necessary for adding unique instances of different subclasses of Item
    public void addWeapon(int id, Weapon weapon) {
        if (weapon.Stackable && IsItemInInventory(weapon)) {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == id) {
                    items[i].Quantity++;
                    InventorySlotData data = slots[i].transform.GetChild(0).GetComponent<InventorySlotData>();
                    data.transform.GetChild(0).GetComponent<Text>().text = items[i].Quantity.ToString();
                }
            }
        } else {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].ID == -1) {
                    currentSlotCount++;
                    items[i] = weapon;
                    items[i].UniqueID = Guid.NewGuid();

                    // Completely UI tweaks, no game functionality
                    GameObject itemObj = Instantiate(inventoryItem);
                    itemObj.GetComponent<InventorySlotData>().item = weapon;
                    itemObj.GetComponent<InventorySlotData>().slot = i;
                    itemObj.GetComponent<InventorySlotData>().inventoryPanelBounds = panelBounds;
                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = slots[i].transform.position;
                    itemObj.GetComponent<Image>().sprite = weapon.Sprite;
                    itemObj.name = weapon.Title;
                    slots[i].name = weapon.Title + " Slot";
                    break;
                }
            }
        }

    }
}

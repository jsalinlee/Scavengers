using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemBehavior: MonoBehaviour {

    public Item item;
    public InventoryController inventoryController;
    
    SpriteRenderer spriteRenderer;
    ItemDatabase database;
    GameObject inventory;

    void Awake() {
        inventory = GameObject.Find("Inventory");
        inventoryController = inventory.GetComponent<InventoryController>();
        database = GameObject.FindObjectOfType<ItemDatabase>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (item != null) {
            Debug.Log("Item before existing: " + item.UniqueID);
        }
    }

    void Start () {
        if (item == null) {
            setItemByID(20);
        }
        Debug.Log("Item is now: " + item.UniqueID);
        spriteRenderer.sprite = item.Sprite;
    }

    public void setItemByID(int idNumber) {
        item = database.FetchItemByID(idNumber);
        Debug.Log(item);
    }

    public void setItemByUniqueID(Guid uniqueIDNumber) {
        for (int i = 0; i < inventoryController.items.Count; i++) {
            if (inventoryController.items[i].UniqueID == uniqueIDNumber) {
                Debug.Log("Item to set: " + inventoryController.items[i].UniqueID);
                item = inventoryController.FetchItemByUniqueID(uniqueIDNumber);
                Debug.Log("Item that was set: " + item.UniqueID);
                break;
            }
        }
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.Sprite;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.transform.tag == "Player" && !inventoryController.IsInventoryFull() && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("Item to add: " + item.UniqueID);
            inventoryController.AddItem(item, item.ID);
            //inventoryController.DropItem(inventoryController.items[1].UniqueID);
            Destroy(gameObject);
        }
    }
}
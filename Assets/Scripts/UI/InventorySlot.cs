using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IDropHandler {
	public int id;
    public GameObject inventoryPanel;

	private InventoryController inv;

	void Start() {
		inv = GameObject.Find("Inventory").GetComponent<InventoryController>();
        inventoryPanel = GameObject.Find("Inventory Panel");
	}
	public void OnDrop(PointerEventData eventData) {
		InventorySlotData droppedItem = eventData.pointerDrag.GetComponent<InventorySlotData>();
		if(inv.items[id].ID == -1) {
			inv.items[droppedItem.slot] = new Item();
			inv.items[id] = droppedItem.item;
			droppedItem.slot = id;
		} else if(droppedItem != null && droppedItem.slot != id) {
			Transform item = this.transform.GetChild(0);
			item.GetComponent<InventorySlotData>().slot = droppedItem.slot;
			item.transform.SetParent(inv.slots[droppedItem.slot].transform);
			item.transform.position = inv.slots[droppedItem.slot].transform.position;

			droppedItem.slot = id;
			droppedItem.transform.SetParent(this.transform);
			droppedItem.transform.position = this.transform.position;

			inv.items[droppedItem.slot] = item.GetComponent<InventorySlotData>().item;
			inv.items[id] = droppedItem.item;
		}
    }
}

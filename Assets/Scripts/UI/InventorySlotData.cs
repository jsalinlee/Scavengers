using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlotData : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
	public Item item;
	public int slot;
    public RectTransform inventoryPanelBounds;

	private InventoryController inv;
	private Tooltip tooltip;
	private Vector2 offset;
    private GameObject inventoryPanel;

	void Start() {
		inv = GameObject.Find("Inventory").GetComponent<InventoryController>();
		tooltip = inv.GetComponent<Tooltip>();
        inventoryPanel = GameObject.Find("Inventory Panel");
	}

	public void OnPointerDown(PointerEventData eventData) {
		if(item != null) {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position - offset;
            inv.slots[slot].name = "Empty Inventory Slot";
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
	}

    public void OnPointerUp(PointerEventData eventData) {
        if (this.transform.parent.parent.transform.Find("Slot Panel") != null) {
            this.transform.SetParent(inv.slots[slot].transform);
            this.transform.position = inv.slots[slot].transform.position;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

	public void OnDrag(PointerEventData eventData) {
		if(item != null) {
			this.transform.position = eventData.position - offset;
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		this.transform.SetParent(inv.slots[slot].transform);
		this.transform.position = inv.slots[slot].transform.position;
        inv.slots[slot].name = this.transform.name + " Slot";
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Rect inventoryBounds = inventoryPanelBounds.rect;
        inventoryBounds.center = inventoryPanel.transform.position;
        if (!inventoryBounds.Contains(eventData.position)) {
            Debug.Log("item to drop: " + item.UniqueID);
            inv.DropItem(item.UniqueID);
        }
	}

	public void OnPointerEnter(PointerEventData eventData) {
		tooltip.Activate(item);
	}

	public void OnPointerExit(PointerEventData eventData) {
		tooltip.Deactivate();
	}
}

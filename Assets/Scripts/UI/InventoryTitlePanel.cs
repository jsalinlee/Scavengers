using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTitlePanel : MonoBehaviour, IPointerDownHandler, IDragHandler {
    GameObject inventoryPanel;
    private Vector2 offset;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    void Start () {
		inventoryPanel = transform.parent.gameObject;
		RectTransform rtPanel = (RectTransform)inventoryPanel.transform;
		RectTransform rtTitle = (RectTransform)transform;
		minX = rtTitle.rect.width / 2;
		maxX = Screen.width - rtTitle.rect.width / 2;
		minY = rtTitle.rect.height / 2 - rtPanel.rect.height / 2;
		maxY = Screen.height - rtTitle.rect.height / 2 - rtPanel.rect.height / 2;
	}
    
    public void OnPointerDown(PointerEventData eventData) {
        offset = eventData.position - new Vector2(inventoryPanel.transform.position.x, inventoryPanel.transform.position.y);
        inventoryPanel.transform.position = eventData.position - offset;
    }

    public void OnDrag(PointerEventData eventData) {
    	// Lock inventory panel to game window
    	float xBounds = Mathf.Clamp(eventData.position.x - offset.x, minX, maxX);
    	float yBounds = Mathf.Clamp(eventData.position.y - offset.y, minY, maxY);
        inventoryPanel.transform.position = new Vector3(xBounds, yBounds, 0);
    }
}

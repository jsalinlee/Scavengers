using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstController : MonoBehaviour {
	Image thirstImage;
	Sprite[] thirstSprites;
	// Use this for initialization
	void Start () {
		thirstImage = GetComponent<Image>();
		thirstSprites = Resources.LoadAll<Sprite>("Sprites/UI/thirst_status");
		thirstImage.sprite = thirstSprites[0];
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerController.thirst < 97 && PlayerController.thirst >= 95) {
			thirstImage.sprite = thirstSprites[1];
		} else if (PlayerController.thirst <= 95) {
			thirstImage.sprite = thirstSprites[2];
		}
	}
}

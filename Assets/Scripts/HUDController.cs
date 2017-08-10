using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    GameObject health;
    Text healthLabel;

    GameObject thirst;
	Image thirstImage;
	Sprite[] thirstSprites;

    public float timer = 0f;
    
	void Start () {
        health = GameObject.Find("Health");
        healthLabel = health.GetComponent<Text>();
        healthLabel.text = "Health: " + PlayerController.health;

        thirst = GameObject.Find("Thirst");
		thirstImage = thirst.GetComponent<Image>();
		thirstSprites = Resources.LoadAll<Sprite>("Sprites/UI/thirst_status");
		thirstImage.sprite = thirstSprites[0];
	}
	
	void Update () {
		CheckThirst();
	}

	void OnGUI () {
		if(GUI.Button(new Rect(10, 10, 100, 25), "Damage")) {
            PlayerController.TakeDamage(10f);
            Debug.Log("Damaged, now: " + PlayerController.health);
            healthLabel.text = "Health: " + PlayerController.health;
        } else if (GUI.Button(new Rect(10, 50, 100, 25), "Heal")) {
            PlayerController.Heal(10f);
            Debug.Log("Healed, now: " + PlayerController.health);
            healthLabel.text = "Health: " + PlayerController.health;
        }
	}

    void CheckThirst() {
        if (PlayerController.thirst <= 97 && PlayerController.thirst > 96) {
			    thirstImage.sprite = thirstSprites[1];
		} else if (PlayerController.thirst <= 96 && PlayerController.thirst > 95) {
			thirstImage.sprite = thirstSprites[2];
		} else if (PlayerController.thirst <= 95 && PlayerController.thirst > 94) {
            thirstImage.sprite = thirstSprites[3];
        } else if (PlayerController.thirst <= 94 && PlayerController.thirst > 93) {
            thirstImage.sprite = thirstSprites[4];
        } else if (PlayerController.thirst <= 93 && PlayerController.thirst > 92) {
            thirstImage.sprite = thirstSprites[5];
        } else if (PlayerController.thirst <= 92) {
            timer += Time.deltaTime;
            if(timer >= 0.5) {
                thirstImage.sprite = thirstSprites[7];
                if (timer >= 1) {
                    timer = 0f;
                }
            } else {
                thirstImage.sprite = thirstSprites[6];
            }
        } else {
            thirstImage.sprite = thirstSprites[0];
        }
    }
}

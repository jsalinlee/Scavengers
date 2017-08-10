using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour {
	GameObject targetUI; // Debug UI to be toggled on/off
	void Start () {
		targetUI = transform.GetChild(0).gameObject;
		targetUI.SetActive(false);
	}
	void Update () {
		if(Input.GetKey((KeyCode)Convert.ToInt32(0x0130)) && Input.GetKeyDown((KeyCode)Convert.ToInt32(0x0030))) // HAHA NICE TRY MY DEBUG MODE IS SECRET
		{
			targetUI.SetActive(!targetUI.activeSelf);
		}
	}
}

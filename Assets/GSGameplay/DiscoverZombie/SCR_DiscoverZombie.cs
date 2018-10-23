using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DiscoverZombie : MonoBehaviour {
	public const float DISPLAY_TIME = 3;
	
	private float displayTime = 0;
	
	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		displayTime += Time.deltaTime;
		if (displayTime >= DISPLAY_TIME) {
			gameObject.SetActive(false);
		}
	}
	
	public void ShowNewZombie() {
		displayTime = 0;
	}
}

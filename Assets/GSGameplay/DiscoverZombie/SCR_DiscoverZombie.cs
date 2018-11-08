using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DiscoverZombie : MonoBehaviour {
	public const float DISPLAY_TIME = 3;
	
	public GameObject[] zombies;
	
	private float displayTime = 0;
	
	public void Start() {
		
	}
	
	public void Update() {
		displayTime += Time.deltaTime;
		if (displayTime >= DISPLAY_TIME) {
			gameObject.SetActive(false);
		}
	}
	
	public void ShowNewZombie(int index) {
		displayTime = 0;
		
		for (int i = 0; i < zombies.Length; i++) {
			zombies[i].SetActive(false);
		}
		
		zombies[index].SetActive(true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Collection : MonoBehaviour {
	public GameObject[] zombies;
	private int currentZombie = 0;
	
	// Use this for initialization
	void Start() {
		UpdateZombie();
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	public void OnArrowLeft() {
		currentZombie--;
		if (currentZombie < 0) currentZombie = zombies.Length - 1;
		UpdateZombie();
	}
	
	public void OnArrowRight() {
		currentZombie++;
		if (currentZombie >= zombies.Length) currentZombie = 0;
		UpdateZombie();
	}
	
	public void UpdateZombie() {
		for (int i = 0; i < zombies.Length; i++) {
			zombies[i].SetActive(false);
		}
		
		zombies[currentZombie].SetActive(true);
	}
}

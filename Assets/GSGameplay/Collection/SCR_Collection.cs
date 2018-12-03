using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Collection : MonoBehaviour {
	public GameObject[] zombies;
	
	public Text txtName;
	public Text txtProductionRate;
	public Text txtDescription;
	
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
		if (currentZombie < 0) currentZombie = SCR_Profile.zombieUnlocked;
		UpdateZombie();
	}
	
	public void OnArrowRight() {
		currentZombie++;
		if (currentZombie > SCR_Profile.zombieUnlocked) currentZombie = 0;
		UpdateZombie();
	}
	
	public void UpdateZombie() {
		for (int i = 0; i < zombies.Length; i++) {
			zombies[i].SetActive(false);
		}
		
		zombies[currentZombie].SetActive(true);
		
		string productionRate = "Brains per second: " + SCR_Gameplay.FormatNumber(SCR_Config.GetProductionRate(currentZombie));
		
		txtName.text = SCR_Config.ZOMBIE_INFO[currentZombie].name.ToUpper();
		txtProductionRate.text = productionRate.ToUpper();
		txtDescription.text = SCR_Config.ZOMBIE_INFO[currentZombie].description.ToUpper();
	}
}

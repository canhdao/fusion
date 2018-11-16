using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_DiscoverZombie : MonoBehaviour {
	public GameObject[] zombies;
	
	public Text txtName;
	
	private float displayTime = 0;
	
	public void Update() {
		displayTime += Time.deltaTime;
		if (displayTime >= SCR_Config.DISCOVER_DISPLAY_TIME) {
			gameObject.SetActive(false);
		}
	}
	
	public void ShowNewZombie(int index) {
		displayTime = 0;
		
		for (int i = 0; i < zombies.Length; i++) {
			zombies[i].SetActive(false);
		}
		
		zombies[index].SetActive(true);
		txtName.text = SCR_Config.ZOMBIE_INFO[index].name;
	}
}

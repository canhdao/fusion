using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_ZombieShop : MonoBehaviour {
	public Text[] zombieNames;
	public Text[] zombieProductionRates;
	public Text[] zombiePrices;
	public Button[] zombieBuyButtons;
	
	public RectTransform content;

	public void Start() {
		for (int i = 0; i < zombiePrices.Length; i++) {
			string productionRate = SCR_Gameplay.FormatNumber(SCR_Config.GetProductionRate(i)) + " brains/s";
			
			zombieNames[i].text = SCR_Config.ZOMBIE_INFO[i].name.ToUpper();
			zombieProductionRates[i].text = productionRate;
			zombiePrices[i].text = SCR_Config.ZOMBIE_INFO[i].price.ToString();
		}
		
		RefreshUnlocked();
		UpdateBrain();
	}
	
	public void RefreshUnlocked() {
		for (int i = 0; i <= SCR_Profile.zombieUnlocked; i++) {
			content.GetChild(i).gameObject.SetActive(true);
		}
		
		for (int i = SCR_Profile.zombieUnlocked + 1; i < content.childCount; i++) {
			content.GetChild(i).gameObject.SetActive(false);
		}
		
		content.sizeDelta = new Vector2(content.sizeDelta.x, 280 + (SCR_Profile.zombieUnlocked + 1) * 250);
	}
	
	public void UpdateBrain() {
		for (int i = 0; i < zombieBuyButtons.Length; i++) {
			if (SCR_Config.ZOMBIE_INFO[i].price <= SCR_Profile.brain) {
				zombieBuyButtons[i].interactable = true;
			}
			else {
				zombieBuyButtons[i].interactable = false;
			}
		}
	}
}

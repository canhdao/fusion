﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UpgradeShop : MonoBehaviour {
	public Text[] zombieLevels;
	public Text[] zombieProductionRates;
	public Text[] zombieUpgradePrices;
	public Button[] zombieUpgradeButtons;
	
	public RectTransform content;

	public void Start() {
		RefreshUnlocked();
		RefreshUpgradeInfo();
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
	
	public void RefreshUpgradeInfo() {
		int total = 0;
		
		for (int i = 0; i < zombieUpgradePrices.Length; i++) {
			int productionRate = SCR_Config.GetProductionRate(i);
			
			zombieLevels[i].text = "LEVEL " + (SCR_Profile.upgradeZombies[i] + 1).ToString();
			zombieProductionRates[i].text = productionRate.ToString() + " brains/s";
			zombieUpgradePrices[i].text = SCR_Gameplay.FormatNumber(SCR_Config.GetUpgradePrice(i));
			
			total += productionRate;
		}
		
		SCR_Gameplay.instance.txtTotalProductionRate.text = SCR_Gameplay.FormatNumber(total) + " brains/s";
	}
	
	public void OnUpgradeZombie(int index) {
		if (SCR_Config.GetUpgradePrice(index) <= SCR_Profile.brain) {
			SCR_Profile.upgradeZombies[index]++;
			SCR_Profile.SaveUpgradeZombies();
			
			RefreshUpgradeInfo();
		}
	}
	
	public void UpdateBrain() {
		for (int i = 0; i < zombieUpgradeButtons.Length; i++) {
			if (SCR_Config.GetUpgradePrice(i) <= SCR_Profile.brain) {
				zombieUpgradeButtons[i].interactable = true;
			}
			else {
				zombieUpgradeButtons[i].interactable = false;
			}
		}
	}
}

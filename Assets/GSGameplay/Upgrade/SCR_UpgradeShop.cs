﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UpgradeShop : MonoBehaviour {
	public Text[] zombieLevels;
	public Text[] zombieProductionRates;
	public Text[] zombieUpgradePrices;
	
	public RectTransform content;

	public void Start() {
		for (int i = 0; i < zombieUpgradePrices.Length; i++) {
			string productionRate = "PRODUCE " + SCR_Config.GetProductionRate(i).ToString() + "/s";
			
			//zombieNames[i].text = SCR_Config.ZOMBIE_INFO[i].name.ToUpper();
			zombieProductionRates[i].text = productionRate;
			//zombiePrices[i].text = SCR_Config.ZOMBIE_INFO[i].price.ToString();
		}
	}
	
	public void Refresh() {
		for (int i = 0; i <= SCR_Profile.zombieUnlocked; i++) {
			content.GetChild(i).gameObject.SetActive(true);
		}
		
		for (int i = SCR_Profile.zombieUnlocked + 1; i < content.childCount; i++) {
			content.GetChild(i).gameObject.SetActive(false);
		}
		
		content.sizeDelta = new Vector2(content.sizeDelta.x, 280 + (SCR_Profile.zombieUnlocked + 1) * 250);
	}
}
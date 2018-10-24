using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_ZombieShop : MonoBehaviour {
	public Text[] zombieNames;
	public Text[] zombieProductionRates;
	public Text[] zombiePrices;

	void Start() {
		for (int i = 0; i < zombiePrices.Length; i++) {
			string productionRate = "PRODUCE " + SCR_Config.ZOMBIE_INFO[i].productionRate.ToString() + "/s";
			
			zombieNames[i].text = SCR_Config.ZOMBIE_INFO[i].name.ToUpper();
			zombieProductionRates[i].text = productionRate;
			zombiePrices[i].text = SCR_Config.ZOMBIE_INFO[i].price.ToString();
		}
	}
}

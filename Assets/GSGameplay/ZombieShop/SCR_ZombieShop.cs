using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_ZombieShop : MonoBehaviour {
	public Text[] zombiePrices;

	void Start() {
		for (int i = 0; i < zombiePrices.Length; i++) {
			zombiePrices[i].text = SCR_Config.ZOMBIE_INFO[i].price.ToString();
		}
	}
}

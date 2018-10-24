using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ZombieInfo {
	public string name;
	public string description;
	public float productionRate;
	public int price;
	
	public ZombieInfo(string _name, string _description, float _productionRate, int _price) {
		name = _name;
		description = _description;
		productionRate = _productionRate;
		price = _price;
	}
}

public class SCR_Config {
	public const int MAX_NUMBER_ZOMBIES	= 16;
	public const int PLUS_BRAIN_AMOUNT	= 10;

	public static ZombieInfo[] ZOMBIE_INFO = new ZombieInfo[] {
		new ZombieInfo("Zombie 1", "Description 1", 0.1f, 1),
		new ZombieInfo("Zombie 2", "Description 2", 0.2f, 2),
		new ZombieInfo("Zombie 3", "Description 3", 0.3f, 3),
		new ZombieInfo("Zombie 4", "Description 4", 0.4f, 4),
		new ZombieInfo("Zombie 5", "Description 5", 0.5f, 5),
		new ZombieInfo("Zombie 6", "Description 6", 0.6f, 6),
		new ZombieInfo("Zombie 7", "Description 7", 0.7f, 7),
		new ZombieInfo("Zombie 8", "Description 8", 0.8f, 8),
		new ZombieInfo("Zombie 9", "Description 9", 0.9f, 9),
		new ZombieInfo("Zombie 10", "Description 10", 1.0f, 10),
		new ZombieInfo("Zombie 11", "Description 11", 1.1f, 11),
		new ZombieInfo("Zombie 12", "Description 12", 1.2f, 12),
		new ZombieInfo("Zombie 13", "Description 13", 1.3f, 13),
		new ZombieInfo("Zombie 14", "Description 14", 1.4f, 14),
		new ZombieInfo("Zombie 15", "Description 15", 1.5f, 15),
	};
}

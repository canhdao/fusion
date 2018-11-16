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
		new ZombieInfo("Crawlie", "Hobos Corporation first experience sample in reviving dead people", 0.1f, 1),
		new ZombieInfo("Siblings", "Sample of zombie's binary fission abilities", 0.2f, 2),
		new ZombieInfo("Gangstar", "Reviving American football player", 0.3f, 3),
		new ZombieInfo("Psycho", "Hobos corporation's failed experiment sample", 0.4f, 4),
		new ZombieInfo("Fatbabe", "Experiment code 02: a giant kid", 0.5f, 5),
		new ZombieInfo("Yogaboy", "Experiment code 001: Human - lizard hybrid", 0.6f, 6),
		new ZombieInfo("Monkey", "Experiment  no 01: Human - European mole hybrid", 0.7f, 7),
		new ZombieInfo("Longbody", "Hybrid experiment between man and woman no 000", 0.8f, 8),
		new ZombieInfo("Gymmer", "Hybrid between a bodybuilder and a child", 0.9f, 9),
		new ZombieInfo("Titan", "First experiment creating the first giant zombie", 1.0f, 10),
		new ZombieInfo("Jombie", "Hobos corporation has tried to create the first super zombie", 1.1f, 11),
		new ZombieInfo("Buddie", "With ambition to dominate the world, Hobos Group continue to experiment the next super zombie generation", 1.2f, 12),
		new ZombieInfo("Shivie", "Power is absolute", 1.3f, 13),
		new ZombieInfo("Zesie", "Show the power of the Almighty", 1.4f, 14),
		new ZombieInfo("Anubie", "Bring the power of an ancient God", 1.5f, 15),
	};
	
	public const float TOMB_SPAWN_INTERVAL = 10.0f;
	public const float DISCOVER_DISPLAY_TIME = 2.5f;
}

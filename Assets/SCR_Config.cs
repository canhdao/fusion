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
		new ZombieInfo("Nor-02",   "Hobos Corporation first experience sample in reviving dead people ", 0.1f, 1),
		new ZombieInfo("Bro-03",   "Sample of zombie's binary fission abilities", 0.2f, 2),
		new ZombieInfo("USA-002",  "Reviving American football player", 0.3f, 3),
		new ZombieInfo("Nor-007",  "Hobos corporation's failed experiment sample", 0.4f, 4),
		new ZombieInfo("Baby-02",  "Experiment code 02: a giant kid", 0.5f, 5),
		new ZombieInfo("Liz-001",  "Experiment code 001: Human - lizard hybrid", 0.6f, 6),
		new ZombieInfo("Talpa-01", "Experiment  no 01: Human - European mole hybrid", 0.7f, 7),
		new ZombieInfo("Bikini",   "Hybrid experiment between man and woman no 000", 0.8f, 8),
		new ZombieInfo("Ryan",     "Hybrid between a bodybuilder and a child", 0.9f, 9),
		new ZombieInfo("Canh",     "First experiment creating the first giant zombie", 1.0f, 10),
		new ZombieInfo("G-003",    "Hobos corporation has tried to create the first super zombie; this is the first experiment", 1.1f, 11),
		new ZombieInfo("GA-007",   "With ambition to dominate the world, Hobos Group continue to experiment the next super zombie generation. ", 1.2f, 12),
		new ZombieInfo("GI-001",   "Power is absolute", 1.3f, 13),
		new ZombieInfo("G-005",    "Show the power of the Almighty ", 1.4f, 14),
		new ZombieInfo("GH-002",   "Bring the power of an ancient God. ", 1.5f, 15),
	};
	
	public const float TOMB_SPAWN_INTERVAL = 10.0f;
	public const float DISCOVER_DISPLAY_TIME = 2.5f;
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ZombieInfo {
	public string name;
	public string description;
	public float baseProductionRate;
	public int price;
	
	public ZombieInfo(string _name, string _description, float _baseProductionRate, int _price) {
		name = _name;
		description = _description;
		baseProductionRate = _baseProductionRate;
		price = _price;
	}
}

public class SCR_Config {
	public const int MAX_NUMBER_ZOMBIES	= 16;

	public static ZombieInfo[] ZOMBIE_INFO = new ZombieInfo[] {
		new ZombieInfo("Crawlie", "Hobos Corporation first experience sample in reviving dead people",
			5, 3000),
		new ZombieInfo("Siblings", "Sample of zombie's binary fission abilities",
			8, 4800),
		new ZombieInfo("Gangstar", "Reviving American football player",
			13, 7800),
		new ZombieInfo("Psycho", "Hobos corporation's failed experiment sample",
			21, 12600),
		new ZombieInfo("Fatbabe", "Experiment code 02: a giant kid",
			32, 19200),
		new ZombieInfo("Yogaboy", "Experiment code 001: Human - lizard hybrid",
			47, 28200),
		new ZombieInfo("Monkey", "Experiment  no 01: Human - European mole hybrid",
			66, 39600),
		new ZombieInfo("Longbody", "Hybrid experiment between man and woman no 000",
			89, 53400),
		new ZombieInfo("Gymmer", "Hybrid between a bodybuilder and a child",
			116, 69600),
		new ZombieInfo("Titan", "First experiment creating the first giant zombie",
			148, 88800),
		new ZombieInfo("Jombie", "Hobos corporation has tried to create the first super zombie",
			184, 110400),
		new ZombieInfo("Buddie", "With ambition to dominate the world, Hobos Group continue to experiment the next super zombie generation",
			226, 135600),
		new ZombieInfo("Shivie", "Power is absolute",
			273, 163800),
		new ZombieInfo("Zesie", "Show the power of the Almighty",
			325, 195000),
		new ZombieInfo("Anubie", "Bring the power of an ancient God",
			383, 229800),
	};
	
	public static int GetProductionRate(int zombieIndex) {
		const float MULTIPLIER = 1.2f;
		int upgradeLevel = SCR_Profile.upgradeZombies[zombieIndex];
		return Mathf.FloorToInt(ZOMBIE_INFO[zombieIndex].baseProductionRate * Mathf.Pow(MULTIPLIER, upgradeLevel));
	}
	
	public static int GetUpgradePrice(int zombieIndex) {
		const float TIME = 300;
		return Mathf.FloorToInt(GetProductionRate(zombieIndex) * TIME);
	}
	
	public const float TOMB_SPAWN_INTERVAL = 10.0f;
	public const float DISCOVER_DISPLAY_TIME = 2.5f;
}

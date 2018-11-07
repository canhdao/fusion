using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Profile {
	public const int NUMBER_ZOMBIES = 15;
	
	public static int brain = 0;
	public static int[] numberZombies = new int[NUMBER_ZOMBIES];
	public static int numberTombs = 0;
	public static int zombieUnlocked = 0;
	
	public static bool finishedTutorial = false;
	
	public static void Load() {
		brain = PlayerPrefs.GetInt("brain", 0);
		
		for (int i = 0; i < NUMBER_ZOMBIES; i++) {
			numberZombies[i] = PlayerPrefs.GetInt("zombie" + i.ToString(), 0);
		}
		
		numberTombs = PlayerPrefs.GetInt("tomb", 0);
		
		zombieUnlocked = PlayerPrefs.GetInt("zombieUnlocked", 0);
		
		finishedTutorial = PlayerPrefs.GetInt("finishedTutorial", 0) == 1;
	}
	
	public static void Save() {
		SaveBrain();
		SaveNumberZombies();
		SaveNumberTombs();
		SaveZombieUnlocked();
		SaveTutorial();
	}
	
	public static void Reset() {
		brain = 0;
		
		for (int i = 0; i < NUMBER_ZOMBIES; i++) {
			numberZombies[i] = 0;
		}
		
		numberTombs = 0;
		
		zombieUnlocked = 0;
		
		finishedTutorial = false;
		
		Save();
	}
	
	public static void SaveBrain() {
		PlayerPrefs.SetInt("brain", brain);
	}
	
	public static void SaveNumberZombies() {		
		for (int i = 0; i < NUMBER_ZOMBIES; i++) {
			PlayerPrefs.SetInt("zombie" + i.ToString(), numberZombies[i]);
		}
	}
	
	public static void SaveNumberTombs() {
		PlayerPrefs.SetInt("tomb", numberTombs);
	}
	
	public static void SaveZombieUnlocked() {
		PlayerPrefs.SetInt("zombieUnlocked", zombieUnlocked);
	}
	
	public static void SaveTutorial() {
		PlayerPrefs.SetInt("finishedTutorial", finishedTutorial ? 1 : 0);
	}
}

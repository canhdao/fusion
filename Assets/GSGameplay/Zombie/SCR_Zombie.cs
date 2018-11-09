using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieType {
	ZOMBIE_1,
	ZOMBIE_2,
	ZOMBIE_3,
	ZOMBIE_4,
	ZOMBIE_5,
	ZOMBIE_6,
	ZOMBIE_7,
	ZOMBIE_8,
	ZOMBIE_9,
	ZOMBIE_10,
	ZOMBIE_11,
	ZOMBIE_12,
	ZOMBIE_13,
	ZOMBIE_14,
	ZOMBIE_15
}

public enum ZombieState {
	AUTO_MOVE,
	USER_MOVE
}

public class SCR_Zombie : MonoBehaviour {
	public const ZombieType LAST_TYPE = ZombieType.ZOMBIE_15;
	
	public const float MOVE_RANGE = 1.0f;
	public const float BRAIN_OFFSET_X = -1.0f;
	public const float BRAIN_OFFSET_Y = 0.0f;
	
	public ZombieType type = ZombieType.ZOMBIE_1;
	public ZombieState state = ZombieState.AUTO_MOVE;
	
	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	public void Move() {
		if (!SCR_Gameplay.instance.showingTutorial) {
			if (state == ZombieState.AUTO_MOVE) {
				float x = transform.localPosition.x + Random.Range(-MOVE_RANGE, MOVE_RANGE);
				float y = transform.localPosition.y + Random.Range(-MOVE_RANGE, MOVE_RANGE);
				
				x = Mathf.Clamp(x, SCR_Gameplay.GARDEN_LEFT, SCR_Gameplay.GARDEN_RIGHT);
				y = Mathf.Clamp(y, SCR_Gameplay.GARDEN_BOTTOM, SCR_Gameplay.GARDEN_TOP);
				
				float z = y;
				
				iTween.MoveTo(gameObject, iTween.Hash("x", x, "y", y, "z", z, "time", 0.75f, "easetype", "easeInOutSine", "islocal", true));
			}
		}
	}
	
	public void StopMoving() {
		iTween.Stop(gameObject);
	}
	
	public void SpawnBrain() {
		float x = transform.localPosition.x + BRAIN_OFFSET_X;
		float y = transform.localPosition.y + BRAIN_OFFSET_Y;
		float z = y;
		
		GameObject brain = Instantiate(SCR_Gameplay.instance.PFB_BRAIN, transform.parent);
		brain.transform.localPosition = new Vector3(x, y, z);
	}
}

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
	ZOMBIE_10
}

public enum ZombieState {
	AUTO_MOVE,
	USER_MOVE
}

public class SCR_Zombie : MonoBehaviour {
	public const ZombieType LAST_TYPE = ZombieType.ZOMBIE_6;
	
	public const float MOVE_RANGE = 1.0f;
	public const float BRAIN_OFFSET_X = -1.0f;
	public const float BRAIN_OFFSET_Y = 0.5f;
	
	public ZombieType type = ZombieType.ZOMBIE_1;
	public ZombieState state = ZombieState.AUTO_MOVE;
	
	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	public void Move() {
		if (state == ZombieState.AUTO_MOVE) {
			float x = transform.position.x + Random.Range(-MOVE_RANGE, MOVE_RANGE);
			float y = transform.position.y + Random.Range(-MOVE_RANGE, MOVE_RANGE);
			
			x = Mathf.Clamp(x, SCR_Gameplay.GARDEN_LEFT, SCR_Gameplay.GARDEN_RIGHT);
			y = Mathf.Clamp(y, SCR_Gameplay.GARDEN_BOTTOM, SCR_Gameplay.GARDEN_TOP);
			
			iTween.MoveTo(gameObject, iTween.Hash("x", x, "y", y, "time", 0.75f, "easetype", "easeInOutSine"));
		}
	}
	
	public void StopMoving() {
		iTween.Stop(gameObject);
	}
	
	public void SpawnBrain() {
		Vector3 position = new Vector3(transform.position.x + BRAIN_OFFSET_X, transform.position.y + BRAIN_OFFSET_Y, transform.position.z);
		Instantiate(SCR_Gameplay.instance.PFB_BRAIN, position, SCR_Gameplay.instance.PFB_BRAIN.transform.rotation);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CowType {
	COW_1,
	COW_2,
	COW_3,
	COW_4,
	COW_5,
	COW_6,
	COW_7,
	COW_8,
	COW_9,
	COW_10
}

public enum CowState {
	AUTO_MOVE,
	USER_MOVE
}

public class SCR_Cow : MonoBehaviour {
	public const CowType LAST_TYPE = CowType.COW_6;
	
	public const float MOVE_RANGE = 1.0f;
	
	public CowType type = CowType.COW_1;
	public CowState state = CowState.AUTO_MOVE;
	
	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	public void Move() {
		if (state == CowState.AUTO_MOVE) {
			float x = transform.position.x + Random.Range(-MOVE_RANGE, MOVE_RANGE);
			float y = transform.position.y + Random.Range(-MOVE_RANGE, MOVE_RANGE);
			
			x = Mathf.Clamp(x, -SCR_Gameplay.SCREEN_WIDTH * 0.5f, SCR_Gameplay.SCREEN_WIDTH * 0.5f);
			y = Mathf.Clamp(y, -SCR_Gameplay.SCREEN_HEIGHT * 0.5f, SCR_Gameplay.SCREEN_HEIGHT * 0.5f);
			
			iTween.MoveTo(gameObject, iTween.Hash("x", x, "y", y, "time", 0.5f, "easetype", "easeInOutSine"));
		}
	}
	
	public void StopMoving() {
		iTween.Stop(gameObject);
	}
}

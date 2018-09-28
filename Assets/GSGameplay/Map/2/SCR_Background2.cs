using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Background2 : MonoBehaviour {
	void Start() {
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			child.position = new Vector3(child.position.x, child.position.y, child.position.y);
		}
	}
}

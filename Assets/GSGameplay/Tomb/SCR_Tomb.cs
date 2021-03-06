﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TombState {
	FALL,
	STAY
}

public class SCR_Tomb : MonoBehaviour {
	public const float GRAVITY = -50.0f;
	
	public TombState state = TombState.FALL;
	
	private float endY = 0;
	private float velocity = 0;
	
	private Animator animator;
	
	void Start() {
		animator = GetComponent<Animator>();
		animator.keepAnimatorControllerStateOnDisable = true;
		
		if (!SCR_Gameplay.instance.showingTutorial || SCR_Gameplay.instance.tutorialPhase != TutorialPhase.OPEN_TOMB) {
			endY = transform.localPosition.y;
			float y = endY + Random.Range(SCR_Gameplay.SCREEN_HEIGHT, SCR_Gameplay.SCREEN_HEIGHT * 2);
			transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
		}
	}
	
	void Update() {
		if (state == TombState.FALL) {
			velocity -= GRAVITY * Time.deltaTime;
			float y = transform.localPosition.y - velocity * Time.deltaTime;
			if (y <= endY) {
				y = endY;
				animator.SetTrigger("bounce");
				state = TombState.STAY;
			}
			float z = y;
			transform.localPosition = new Vector3(transform.localPosition.x, y, z);
		}
	}
}

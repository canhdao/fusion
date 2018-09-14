using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrateState {
	FALL,
	STAY
}

public class SCR_Crate : MonoBehaviour {
	public const float GRAVITY = -50.0f;
	
	public GameObject PFB_COW;
	
	private float endY = 0;
	private float velocity = 0;
	
	private CrateState state = CrateState.FALL;
	private Animator animator;

	// Use this for initialization
	void Start() {
		animator = GetComponent<Animator>();
		
		endY = transform.position.y - SCR_Gameplay.SCREEN_HEIGHT;
	}
	
	// Update is called once per frame
	void Update() {
		if (state == CrateState.FALL) {
			velocity -= GRAVITY * Time.deltaTime;
			float y = transform.position.y - velocity * Time.deltaTime;
			if (y <= endY) {
				y = endY;
				animator.SetTrigger("bounce");
				state = CrateState.STAY;
			}
			transform.position = new Vector3(transform.position.x, y, transform.position.z);
		}
	}
	
	public void Open() {
		Instantiate(PFB_COW, transform.position, PFB_COW.transform.rotation);
		Destroy(gameObject);
	}
}

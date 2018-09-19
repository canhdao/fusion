using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TombState {
	FALL,
	STAY
}

public class SCR_Tomb : MonoBehaviour {
	public const float GRAVITY = -50.0f;
	
	public GameObject PFB_ZOMBIE;
	
	private float endY = 0;
	private float velocity = 0;
	
	private TombState state = TombState.FALL;
	private Animator animator;

	// Use this for initialization
	void Start() {
		animator = GetComponent<Animator>();
		
		endY = transform.position.y - SCR_Gameplay.SCREEN_HEIGHT;
	}
	
	// Update is called once per frame
	void Update() {
		if (state == TombState.FALL) {
			velocity -= GRAVITY * Time.deltaTime;
			float y = transform.position.y - velocity * Time.deltaTime;
			if (y <= endY) {
				y = endY;
				animator.SetTrigger("bounce");
				state = TombState.STAY;
			}
			transform.position = new Vector3(transform.position.x, y, transform.position.z);
		}
	}
	
	public void Open() {
		Instantiate(PFB_ZOMBIE, transform.position, PFB_ZOMBIE.transform.rotation);
		Destroy(gameObject);
	}
}

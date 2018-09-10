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

public class SCR_Cow : MonoBehaviour {
	public const CowType LAST_TYPE = CowType.COW_6;
	
	public const float BIG_SCALE = 1.25f;
	public const float SMALL_SCALE = 0.8f;
	
	public const float SCALE_BIG_DURATION = 1.0f;
	public const float SCALE_SMALL_DURATION = 0.25f;
	
	public CowType type;
	
	private float startScaleX;
	private float startScaleY;
	
	// Use this for initialization
	void Start() {
		startScaleX = transform.localScale.x;
		startScaleY = transform.localScale.y;
		StartPhase1();
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	public void UpdateScale(float scale) {
		transform.localScale = new Vector3(startScaleX * scale, startScaleY * scale, 1);
	}
	
	public void StartPhase1() {
		iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", BIG_SCALE, "time", SCALE_BIG_DURATION, "onupdate", "UpdateScale", "oncomplete", "CompletePhase1"));
	}
	
	public void CompletePhase1() {
		iTween.ValueTo(gameObject, iTween.Hash("from", BIG_SCALE, "to", 1.0f, "time", SCALE_BIG_DURATION, "onupdate", "UpdateScale", "oncomplete", "CompletePhase2"));
	}
	
	public void CompletePhase2() {
		iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", SMALL_SCALE, "time", SCALE_SMALL_DURATION, "onupdate", "UpdateScale", "oncomplete", "CompletePhase3"));
	}
	
	public void CompletePhase3() {
		iTween.ValueTo(gameObject, iTween.Hash("from", SMALL_SCALE, "to", 1.0f, "time", SCALE_SMALL_DURATION, "onupdate", "UpdateScale", "oncomplete", "CompletePhase4"));
	}
	
	public void CompletePhase4() {
		StartPhase1();
	}
}

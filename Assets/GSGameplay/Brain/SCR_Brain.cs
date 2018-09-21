using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Brain : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
	
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "delay", 0.5f, "onupdate", "UpdateAlpha", "oncomplete", "Destroy"));

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void UpdateAlpha(float alpha) {
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
	}
	
	private void Destroy() {
		Destroy(gameObject);
	}
}

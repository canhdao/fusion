using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_PlusBrain : MonoBehaviour {
	public const float MOVE_UP_DISTANCE = 100.0f;
	
	private Text text;
	private RectTransform rectTransform;
	
	void Start() {
		text = GetComponent<Text>();
		rectTransform = GetComponent<RectTransform>();
		
		iTween.ValueTo(gameObject, iTween.Hash("from", rectTransform.anchoredPosition.y, "to", rectTransform.anchoredPosition.y + MOVE_UP_DISTANCE, "time", 1.0f, "easetype", "easeOutSine", "onupdate", "UpdateY"));
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.5f, "delay", 0.5f, "onupdate", "UpdateAlpha", "oncomplete", "AutoDestroy"));
	}
	
	private void UpdateAlpha(float alpha) {
		text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
	}
	
	private void AutoDestroy() {
		Destroy(gameObject);
	}
	
	private void UpdateY(float y) {
		rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Brain : MonoBehaviour {
	public const float PLUS_BRAIN_OFFSET_Y = 100.0f;
	
	public GameObject PFB_PLUS_BRAIN;
	private SpriteRenderer spriteRenderer;
	private int brainValue;
	
	public void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.25f, "delay", 0.5f, "onupdate", "UpdateAlpha", "oncomplete", "OnCompleteFadeOut"));
	}
	
	public void SetValue(int v) {
		brainValue = v;
	}
	
	private void UpdateAlpha(float alpha) {
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
	}
	
	private void OnCompleteFadeOut() {
		if (SCR_Gameplay.instance.cvsGameplay.activeSelf) {
			GameObject plusBrain = Instantiate(PFB_PLUS_BRAIN);
			
			RectTransform rectTransform = plusBrain.GetComponent<RectTransform>();
			RectTransform canvasRT = SCR_Gameplay.instance.cvsGameplay.GetComponent<RectTransform>();
			rectTransform.SetParent(canvasRT, false);
			rectTransform.SetAsFirstSibling();
			
			float x = transform.position.x / SCR_Gameplay.SCREEN_WIDTH * canvasRT.rect.width;
			float y = transform.position.y / SCR_Gameplay.SCREEN_HEIGHT * canvasRT.rect.height + PLUS_BRAIN_OFFSET_Y;
			rectTransform.anchoredPosition = new Vector2(x, y);
			
			plusBrain.GetComponent<Text>().text = "+" + SCR_Gameplay.FormatNumber(brainValue);
		}
		
		Destroy(gameObject);
	}
	
	public void OnDisable() {
		Destroy(gameObject);
	}
}

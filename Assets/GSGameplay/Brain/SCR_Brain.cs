using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Brain : MonoBehaviour {
	public const float PLUS_BRAIN_OFFSET_Y = 100.0f;
	
	public GameObject PFB_PLUS_BRAIN;
	private SpriteRenderer spriteRenderer;
	
	// Use this for initialization
	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.25f, "delay", 0.5f, "onupdate", "UpdateAlpha", "oncomplete", "OnCompleteFadeOut"));
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	private void UpdateAlpha(float alpha) {
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
	}
	
	private void OnCompleteFadeOut() {
		GameObject plusBrain = Instantiate(PFB_PLUS_BRAIN);
		
		RectTransform rectTransform = plusBrain.GetComponent<RectTransform>();
		RectTransform canvasRT = SCR_Gameplay.instance.cvsGameplay.GetComponent<RectTransform>();
		rectTransform.SetParent(canvasRT, false);
		
		float x = transform.position.x / SCR_Gameplay.SCREEN_WIDTH * canvasRT.rect.width;
		float y = transform.position.y / SCR_Gameplay.SCREEN_HEIGHT * canvasRT.rect.height + PLUS_BRAIN_OFFSET_Y;
		rectTransform.anchoredPosition = new Vector2(x, y);
		
		plusBrain.GetComponent<Text>().text = "+" + SCR_Config.PLUS_BRAIN_AMOUNT.ToString();
		
		SCR_Gameplay.instance.IncreaseBrain(SCR_Config.PLUS_BRAIN_AMOUNT);
		
		Destroy(gameObject);
	}
}

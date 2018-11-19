using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_AutoFade : MonoBehaviour {
	public Image imgDarken;
	public Text txtTitle;
	public Text txtContent;
	
	private float darkenAlpha = 0;
	
	public void Start() {
		darkenAlpha = imgDarken.color.a;
		UpdateAlpha(0);
	}
	
	public void OnEnable() {
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.2f, "onupdate", "UpdateAlpha"));
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.2f, "delay", 2.5f, "onupdate", "UpdateAlpha", "oncomplete", "AutoDeactivate"));
	}
	
	public void UpdateAlpha(float alpha) {
		imgDarken.color = new Color(imgDarken.color.r, imgDarken.color.g, imgDarken.color.b, alpha * darkenAlpha);
		txtTitle.color = new Color(txtTitle.color.r, txtTitle.color.g, txtTitle.color.b, alpha);
		txtContent.color = new Color(txtContent.color.r, txtContent.color.g, txtContent.color.b, alpha);
	}
	
	public void AutoDeactivate() {
		gameObject.SetActive(false);
	}
}

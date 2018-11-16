using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_AreaIsFull : MonoBehaviour {
	public Image imgDarken;
	public Text txtFull;
	public Text txtMerge;
	
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
		txtFull.color = new Color(txtFull.color.r, txtFull.color.g, txtFull.color.b, alpha);
		txtMerge.color = new Color(txtMerge.color.r, txtMerge.color.g, txtMerge.color.b, alpha);
	}
	
	public void AutoDeactivate() {
		gameObject.SetActive(false);
	}
}

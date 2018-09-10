using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Crate : MonoBehaviour {
	public GameObject PFB_COW;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	public void Open() {
		Instantiate(PFB_COW, transform.position, PFB_COW.transform.rotation);
		Destroy(gameObject);
	}
}

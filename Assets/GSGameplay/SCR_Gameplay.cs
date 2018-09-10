﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Gameplay : MonoBehaviour {
	public const float CRATE_SPAWN_INTERVAL = 3.0f;
	
	public static float SCREEN_WIDTH;
	public static float SCREEN_HEIGHT;
	
	public GameObject PFB_CRATE;
	public GameObject PFB_COW;
	
	private float crateSpawnTime = 0;
	
	private Transform selectedCow = null;
	private float offsetX = 0;
	private float offsetY = 0;
	
	// Use this for initialization
	void Start() {
		SCREEN_HEIGHT = Camera.main.orthographicSize * 2;
		SCREEN_WIDTH = SCREEN_HEIGHT * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D bestHit = FindBestHit(pos);
			if (bestHit.transform != null) {
				SCR_Crate scrCrate = bestHit.transform.GetComponent<SCR_Crate>();
				if (scrCrate != null) {
					scrCrate.Open();
				}
				
				SCR_Cow scrCow = bestHit.transform.GetComponent<SCR_Cow>();
				if (scrCow != null) {
					selectedCow = bestHit.transform;
					offsetX = selectedCow.position.x - pos.x;
					offsetY = selectedCow.position.y - pos.y;
					selectedCow.GetComponent<Collider2D>().enabled = false;
				}
			}
		}
		
		if (Input.GetMouseButton(0)) {
			if (selectedCow != null) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				selectedCow.position = new Vector3(pos.x + offsetX, pos.y + offsetY, 0);
			}
		}
		
		if (Input.GetMouseButtonUp(0)) {
			if (selectedCow != null) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D bestHit = FindBestHit(pos);
				if (bestHit.transform != null) {
					SCR_Cow scrCow = bestHit.transform.GetComponent<SCR_Cow>();
					if (scrCow != null) {
						FuseCow(selectedCow.gameObject, scrCow.gameObject);
					}
				}
				
				selectedCow.GetComponent<Collider2D>().enabled = true;
				selectedCow = null;
			}
		}

		crateSpawnTime += Time.deltaTime;
		if (crateSpawnTime >= CRATE_SPAWN_INTERVAL) {
			SpawnCrate();
			crateSpawnTime = 0;
		}
	}
	
	public void SpawnCrate() {
		float x = Random.Range(-SCREEN_WIDTH * 0.5f, SCREEN_WIDTH * 0.5f);
		float y = Random.Range(-SCREEN_HEIGHT * 0.5f, SCREEN_HEIGHT * 0.5f);
		
		Vector3 position = new Vector3(x, y, PFB_CRATE.transform.position.z);
		Instantiate(PFB_CRATE, position, PFB_CRATE.transform.rotation);
	}
	
	public RaycastHit2D FindBestHit(Vector3 pos) {
		RaycastHit2D bestHit = new RaycastHit2D();

		RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);
		
		float minSqrDistance = -1;
		
		foreach (RaycastHit2D hit in hits) {
			Vector3 distance = hit.transform.position - pos;
			float sqrDistance = distance.sqrMagnitude;
			if (minSqrDistance < 0 || minSqrDistance > sqrDistance) {
				minSqrDistance = sqrDistance;
				bestHit = hit;
			}
		}
		
		return bestHit;
	}
	
	public void FuseCow(GameObject cow1, GameObject cow2) {
		Vector3 position = (cow1.transform.position + cow2.transform.position) * 0.5f;
		GameObject cow = Instantiate(PFB_COW, position, PFB_COW.transform.rotation);
		cow.transform.localScale = new Vector3(cow1.transform.localScale.x + cow2.transform.localScale.x,
			cow1.transform.localScale.y + cow2.transform.localScale.y, 1);
		Destroy(cow1);
		Destroy(cow2);
	}
}

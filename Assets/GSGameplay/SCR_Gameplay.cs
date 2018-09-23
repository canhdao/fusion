using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Gameplay : MonoBehaviour {
	public const float TOMB_SPAWN_INTERVAL = 3.0f;
	
	public static float SCREEN_WIDTH;
	public static float SCREEN_HEIGHT;

	public static float GARDEN_X;
	public static float GARDEN_Y;

	public static float GARDEN_WIDTH;
	public static float GARDEN_HEIGHT;

	public static float GARDEN_LEFT;
	public static float GARDEN_RIGHT;
	public static float GARDEN_TOP;
	public static float GARDEN_BOTTOM;
	
	public static SCR_Gameplay instance;
	
	public GameObject PFB_TOMB;
	public GameObject[] PFB_ZOMBIES;
	public GameObject PFB_BRAIN;
	public GameObject PFB_FUSE_EFFECT;

	public GameObject cvsGameplay;
	public GameObject garden;
	public GameObject[] backgrounds;
	public GameObject zombieShop;
	
	public Text txtBrain;

	public int numberZombies = 0;
	
	private float tombSpawnTime = TOMB_SPAWN_INTERVAL;
	
	private Transform selectedZombie = null;
	private float offsetX = 0;
	private float offsetY = 0;
	
	private int brain = 0;
	
	void Awake() {
		instance = this;
	}
	
	// Use this for initialization
	void Start() {
		SCREEN_HEIGHT = Camera.main.orthographicSize * 2;
		SCREEN_WIDTH = SCREEN_HEIGHT * Screen.width / Screen.height;

		GARDEN_X = garden.transform.position.x;
		GARDEN_Y = garden.transform.position.y;

		GARDEN_WIDTH = garden.transform.localScale.x;
		GARDEN_HEIGHT = garden.transform.localScale.y;

		GARDEN_LEFT = GARDEN_X - GARDEN_WIDTH * 0.5f;
		GARDEN_RIGHT = GARDEN_X + GARDEN_WIDTH * 0.5f;
		GARDEN_TOP = GARDEN_Y + GARDEN_HEIGHT * 0.5f;
		GARDEN_BOTTOM = GARDEN_Y - GARDEN_HEIGHT * 0.5f;
		
		brain = PlayerPrefs.GetInt("brain", 0);
		txtBrain.text = brain.ToString();

		backgrounds[0].SetActive(true);

		for (int i = 1; i < backgrounds.Length; i++) {
			backgrounds[i].SetActive(false);
		}

		zombieShop.SetActive(false);
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D bestHit = FindBestHit(pos);
			if (bestHit.transform != null) {
				if (bestHit.transform.parent != null) {
					SCR_Tomb scrTomb = bestHit.transform.parent.GetComponent<SCR_Tomb>();
					if (scrTomb != null) {
						scrTomb.Open();
					}
				}
				
				SCR_Zombie scrZombie = bestHit.transform.GetComponent<SCR_Zombie>();
				if (scrZombie != null) {
					selectedZombie = bestHit.transform;
					offsetX = selectedZombie.position.x - pos.x;
					offsetY = selectedZombie.position.y - pos.y;
					selectedZombie.GetComponent<Collider2D>().enabled = false;
					
					scrZombie.StopMoving();
					scrZombie.state = ZombieState.USER_MOVE;
				}
			}
		}
		
		if (Input.GetMouseButton(0)) {
			if (selectedZombie != null) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				float x = Mathf.Clamp(pos.x + offsetX, GARDEN_LEFT, GARDEN_RIGHT);
				float y = Mathf.Clamp(pos.y + offsetY, GARDEN_BOTTOM, GARDEN_TOP);
				float z = y;
				selectedZombie.position = new Vector3(x, y, z);
			}
		}
		
		if (Input.GetMouseButtonUp(0)) {
			if (selectedZombie != null) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D bestHit = FindBestHit(pos);
				if (bestHit.transform != null) {
					SCR_Zombie scrZombie = bestHit.transform.GetComponent<SCR_Zombie>();
					if (scrZombie != null) {
						if (selectedZombie.GetComponent<SCR_Zombie>().type == scrZombie.type && scrZombie.type != SCR_Zombie.LAST_TYPE) {
							FuseZombie(selectedZombie.gameObject, scrZombie.gameObject);
						}
					}
				}
				
				selectedZombie.GetComponent<SCR_Zombie>().state = ZombieState.AUTO_MOVE;
				
				selectedZombie.GetComponent<Collider2D>().enabled = true;
				selectedZombie = null;
			}
		}

		tombSpawnTime += Time.deltaTime;
		if (tombSpawnTime >= TOMB_SPAWN_INTERVAL && numberZombies < SCR_Config.MAX_NUMBER_ZOMBIES) {
			SpawnTomb();
			tombSpawnTime = 0;
		}
	}
	
	public void SpawnTomb() {
		float x = Random.Range(GARDEN_LEFT, GARDEN_RIGHT);
		float y = Random.Range(GARDEN_BOTTOM, GARDEN_TOP) + SCREEN_HEIGHT;
		float z = y;
		
		Vector3 position = new Vector3(x, y, z);
		Instantiate(PFB_TOMB, position, PFB_TOMB.transform.rotation);

		numberZombies++;
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
	
	public void FuseZombie(GameObject zombie1, GameObject zombie2) {
		Vector3 position = (zombie1.transform.position + zombie2.transform.position) * 0.5f;
		int zombieIndex = (int)zombie1.GetComponent<SCR_Zombie>().type + 1;
		
		// TEST
		// if (zombieIndex == 1) zombieIndex = 7;

		Instantiate(PFB_ZOMBIES[zombieIndex], position, PFB_ZOMBIES[zombieIndex].transform.rotation);
		Instantiate(PFB_FUSE_EFFECT, position, PFB_FUSE_EFFECT.transform.rotation);
		
		Destroy(zombie1);
		Destroy(zombie2);

		numberZombies--;
	}
	
	public void IncreaseBrain(int amount) {
		brain += amount;
		txtBrain.text = brain.ToString();
		PlayerPrefs.SetInt("brain", brain);
	}

	public void SwitchMap(int map) {
		for (int i = 0; i < backgrounds.Length; i++) {
			backgrounds[i].SetActive(false);
		}

		backgrounds[map - 1].SetActive(true);
	}

	public void OpenZombieShop() {
		zombieShop.SetActive(true);
	}

	public void CloseZombieShop() {
		zombieShop.SetActive(false);
	}
}

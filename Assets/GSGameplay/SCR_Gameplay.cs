using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialPhase {
	OPEN_TOMB,
	OPEN_ZOMBIE_SHOP,
	BUY_ZOMBIE,
	CLOSE_ZOMBIE_SHOP,
	EVOLVE_ZOMBIE
}

public class SCR_Gameplay : MonoBehaviour {
	public const float TOMB_SPAWN_INTERVAL = 3.0f;
	
	public const float HAND_TOMB_OFFSET_X = 50.0f;
	public const float HAND_TOMB_OFFSET_Y = -40;
	
	public const float HAND_ZOMBIE_OFFSET_X = 50.0f;
	public const float HAND_ZOMBIE_OFFSET_Y = -50;
	
	public const float HAND_BUTTON_OFFSET_X = 70.0f;
	public const float HAND_BUTTON_OFFSET_Y = -100.0f;
	
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
	public GameObject cvsUpgrade;
	public GameObject cvsCollection;
	public GameObject cvsDiscover;
	public GameObject garden;
	public GameObject[] backgrounds;
	public GameObject zombieShop;
	public GameObject hand;
	public GameObject btnZombieShop;
	public GameObject btnBuyZombie1;
	public GameObject btnCloseZombieShop;
	
	public Text txtBrain;

	public int numberZombies = 0;
	
	public bool showingTutorial = true;
	
	private float tombSpawnTime = TOMB_SPAWN_INTERVAL;
	
	private Transform selectedZombie = null;
	private float offsetX = 0;
	private float offsetY = 0;
	
	private int brain = 0;

	private int currentMap = 1;
	private int nextMap = 1;
	
	private bool switchingMap = false;

	private TutorialPhase tutorialPhase = TutorialPhase.OPEN_TOMB;
	private Vector3 tutorialZombiePosition1 = Vector3.zero;
	private Vector3 tutorialZombiePosition2 = Vector3.zero;
	
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

		if (showingTutorial) {
			ShowTutorial(TutorialPhase.OPEN_TOMB);
		}
		else {
			hand.SetActive(false);
		}

		cvsGameplay.SetActive(true);
		cvsUpgrade.SetActive(false);
		cvsCollection.SetActive(false);
		cvsDiscover.SetActive(false);
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
						GameObject zombie = Instantiate(PFB_ZOMBIES[0], backgrounds[currentMap - 1].transform);
						zombie.transform.position = bestHit.transform.parent.position;
						Destroy(bestHit.transform.parent.gameObject);

						if (showingTutorial) {
							if (tutorialPhase == TutorialPhase.OPEN_TOMB) {
								tutorialZombiePosition1 = zombie.transform.position;
								ShowTutorial(TutorialPhase.OPEN_ZOMBIE_SHOP);
							}
						}
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

		if (!showingTutorial) {
			tombSpawnTime += Time.deltaTime;
			if (tombSpawnTime >= TOMB_SPAWN_INTERVAL && numberZombies < SCR_Config.MAX_NUMBER_ZOMBIES) {
				SpawnTomb();
				tombSpawnTime = 0;
			}
		}
	}
	
	public void SpawnTomb() {
		float x = Random.Range(GARDEN_LEFT, GARDEN_RIGHT);
		float y = Random.Range(GARDEN_BOTTOM, GARDEN_TOP) + SCREEN_HEIGHT;
		float z = y;
		
		GameObject tomb = Instantiate(PFB_TOMB, backgrounds[0].transform);
		tomb.transform.position = new Vector3(x, y, z);

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
		int zombieIndex = (int)zombie1.GetComponent<SCR_Zombie>().type + 1;

		int map = 1;
		if (zombieIndex <= 4) {
			map = 1;
		}
		else if (zombieIndex <= 9) {
			map = 2;
		}
		else if (zombieIndex <= 14) {
			map = 3;
		}
		
		Vector3 position = (zombie1.transform.position + zombie2.transform.position) * 0.5f;
		
		GameObject zombie = Instantiate(PFB_ZOMBIES[zombieIndex], backgrounds[map - 1].transform);
		zombie.transform.position = position;
		Instantiate(PFB_FUSE_EFFECT, position, PFB_FUSE_EFFECT.transform.rotation);
		
		Destroy(zombie1);
		Destroy(zombie2);

		numberZombies--;
		
		if (showingTutorial) {
			if (tutorialPhase == TutorialPhase.EVOLVE_ZOMBIE) {
				hand.SetActive(false);
				showingTutorial = false;
			}
		}
		
		// check if it's a new zombie...
		cvsDiscover.SetActive(true);
	}
	
	public void IncreaseBrain(int amount) {
		brain += amount;
		txtBrain.text = brain.ToString();
		PlayerPrefs.SetInt("brain", brain);
	}

	public void DecreaseBrain(int amount) {
		brain -= amount;
		txtBrain.text = brain.ToString();
		PlayerPrefs.SetInt("brain", brain);
	}

	public void SwitchMap(int map) {
		if (!switchingMap && map != currentMap) {
			switchingMap = true;
			nextMap = map;
			
			if (currentMap < map) {
				DisappearShrinkMap(currentMap);
			}

			if (currentMap > map) {
				DisappearEnlargeMap(currentMap);
			}
		}
	}

	public void DisappearShrinkMap(int map) {
		iTween.ScaleTo(backgrounds[currentMap - 1], iTween.Hash("x", 0.01f, "y", 0.01f, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteDisappearShrinkMap", "oncompletetarget", gameObject));
	}

	public void AppearShrinkMap(int map) {
		backgrounds[map - 1].SetActive(true);
		backgrounds[map - 1].transform.localScale = new Vector3(10, 10, 1);
		iTween.ScaleTo(backgrounds[map - 1], iTween.Hash("x", 1, "y", 1, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteSwitchMap", "oncompletetarget", gameObject));
	}

	public void CompleteDisappearShrinkMap() {
		backgrounds[currentMap - 1].SetActive(false);
		AppearShrinkMap(nextMap);
	}

	public void DisappearEnlargeMap(int map) {
		iTween.ScaleTo(backgrounds[currentMap - 1], iTween.Hash("x", 10, "y", 10, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteDisappearEnlargeMap", "oncompletetarget", gameObject));
	}

	public void AppearEnlargeMap(int map) {
		backgrounds[map - 1].SetActive(true);
		backgrounds[map - 1].transform.localScale = new Vector3(0.01f, 0.01f, 1);
		iTween.ScaleTo(backgrounds[map - 1], iTween.Hash("x", 1, "y", 1, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteSwitchMap", "oncompletetarget", gameObject));
	}

	public void CompleteDisappearEnlargeMap() {
		backgrounds[currentMap - 1].SetActive(false);
		AppearEnlargeMap(nextMap);
	}

	public void CompleteSwitchMap() {
		currentMap = nextMap;
		switchingMap = false;
	}

	public void OpenZombieShop() {
		zombieShop.SetActive(true);
		
		if (showingTutorial) {
			if (tutorialPhase == TutorialPhase.OPEN_ZOMBIE_SHOP) {
				ShowTutorial(TutorialPhase.BUY_ZOMBIE);
			}
		}
	}

	public void CloseZombieShop() {
		zombieShop.SetActive(false);
		
		if (showingTutorial) {
			if (tutorialPhase == TutorialPhase.CLOSE_ZOMBIE_SHOP) {
				ShowTutorial(TutorialPhase.EVOLVE_ZOMBIE);
			}
		}
	}

	public void OpenUpgrade() {
		cvsGameplay.SetActive(false);
		cvsUpgrade.SetActive(true);
	}

	public void CloseUpgrade() {
		cvsUpgrade.SetActive(false);
		cvsGameplay.SetActive(true);
	}

	public void OpenCollection() {
		cvsGameplay.SetActive(false);
		cvsCollection.SetActive(true);
	}

	public void CloseCollection() {
		cvsCollection.SetActive(false);
		cvsGameplay.SetActive(true);
	}

	public void BuyZombie(int index) {
		if (numberZombies < SCR_Config.MAX_NUMBER_ZOMBIES && brain >= SCR_Config.ZOMBIE_PRICES[index]) {
			float x = Random.Range(GARDEN_LEFT, GARDEN_RIGHT);
			float y = Random.Range(GARDEN_BOTTOM, GARDEN_TOP);
			float z = y;
			
			int map = 1;
			if (index <= 4) {
				map = 1;
			}
			else if (index <= 9) {
				map = 2;
			}
			else if (index <= 14) {
				map = 3;
			}
			
			GameObject zombie = Instantiate(PFB_ZOMBIES[index], backgrounds[map - 1].transform);
			zombie.transform.position = new Vector3(x, y, z);

			numberZombies++;

			DecreaseBrain(SCR_Config.ZOMBIE_PRICES[index]);
			
			if (showingTutorial) {
				if (tutorialPhase == TutorialPhase.BUY_ZOMBIE) {
					if (index == 0) {
						tutorialZombiePosition2 = zombie.transform.position;
						ShowTutorial(TutorialPhase.CLOSE_ZOMBIE_SHOP);
					}
				}
			}
		}
	}

	private void ShowTutorial(TutorialPhase phase) {
		tutorialPhase = phase;
		
		if (tutorialPhase == TutorialPhase.OPEN_TOMB) {
			float x = Random.Range(GARDEN_LEFT * 0.5f, GARDEN_RIGHT * 0.5f);
			float y = Random.Range(GARDEN_BOTTOM * 0.5f, GARDEN_TOP * 0.5f);
			
			GameObject tomb = Instantiate(PFB_TOMB, backgrounds[0].transform);
			tomb.transform.position = new Vector3(x, y, y);
			tomb.GetComponent<SCR_Tomb>().state = TombState.STAY;
			
			x = x * 100 + HAND_TOMB_OFFSET_X;
			y = y * 100 + HAND_TOMB_OFFSET_Y;

			hand.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
			hand.SetActive(true);

			numberZombies++;
		}
		
		if (tutorialPhase == TutorialPhase.OPEN_ZOMBIE_SHOP) {
			RectTransform buttonRT = btnZombieShop.GetComponent<RectTransform>();
			RectTransform handRT = hand.GetComponent<RectTransform>();
			
			handRT.anchoredPosition = GetHandPositionFromRT(buttonRT);
		}
		
		if (tutorialPhase == TutorialPhase.BUY_ZOMBIE) {
			RectTransform buttonRT = btnBuyZombie1.GetComponent<RectTransform>();
			RectTransform handRT = hand.GetComponent<RectTransform>();
			
			handRT.anchoredPosition = GetHandPositionFromRT(buttonRT);
		}
		
		if (tutorialPhase == TutorialPhase.CLOSE_ZOMBIE_SHOP) {
			RectTransform buttonRT = btnCloseZombieShop.GetComponent<RectTransform>();
			RectTransform handRT = hand.GetComponent<RectTransform>();
			
			handRT.anchoredPosition = GetHandPositionFromRT(buttonRT);
		}
		
		if (tutorialPhase == TutorialPhase.EVOLVE_ZOMBIE) {
			hand.GetComponent<Animator>().SetTrigger("drag");
			iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "UpdateHandPosition", "looptype", "loop"));
		}
	}
	
	public void UpdateHandPosition(float t) {
		float x = (tutorialZombiePosition2.x - tutorialZombiePosition1.x) * t + tutorialZombiePosition1.x;
		float y = (tutorialZombiePosition2.y - tutorialZombiePosition1.y) * t + tutorialZombiePosition1.y;
		
		x = x * 100 + HAND_ZOMBIE_OFFSET_X;
		y = y * 100 + HAND_ZOMBIE_OFFSET_Y;
		
		hand.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
	}
	
	public Vector2 GetHandPositionFromRT(RectTransform rt) {
		Vector3[] worldCorners = new Vector3[4];
		rt.GetWorldCorners(worldCorners);
		
		Vector3 bottomLeft = worldCorners[0];
		Vector3 bottomRight = worldCorners[3];
		Vector3 topLeft = worldCorners[1];
		
		float x = (bottomLeft.x + bottomRight.x - Screen.width) * 0.5f * SCREEN_WIDTH * 100 / Screen.width + HAND_BUTTON_OFFSET_X;
		float y = (bottomLeft.y + topLeft.y - Screen.height) * 0.5f * SCREEN_HEIGHT * 100 / Screen.height + HAND_BUTTON_OFFSET_Y;
		return new Vector2(x, y);
	}
}

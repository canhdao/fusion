using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public enum TutorialPhase {
	OPEN_TOMB,
	OPEN_ZOMBIE_SHOP,
	BUY_ZOMBIE,
	CLOSE_ZOMBIE_SHOP,
	EVOLVE_ZOMBIE
}

public class SCR_Gameplay : MonoBehaviour {
	public const float HAND_TOMB_OFFSET_X = 50.0f;
	public const float HAND_TOMB_OFFSET_Y = -40;
	
	public const float HAND_ZOMBIE_OFFSET_X = 50.0f;
	public const float HAND_ZOMBIE_OFFSET_Y = -50;
	
	public const float HAND_BUTTON_OFFSET_X = 70.0f;
	public const float HAND_BUTTON_OFFSET_Y = -100.0f;
	
	public const float BANNER_GAMEPLAY_Y = 0.1f;
	
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
	public GameObject cvsCoin;
	public GameObject cvsDiscover;
	public GameObject garden;
	public GameObject[] backgrounds;
	public GameObject zombieShop;
	public GameObject hand;
	public GameObject btnZombieShop;
	public GameObject btnBuyZombie1;
	public GameObject btnCloseZombieShop;
	public GameObject grpAreaIsFull;
	public GameObject grpPerfect;
	
	public Text txtBrain;
	public Text txtTotalProductionRate;
	
	public AudioSource	source;
	
	public AudioClip	sndButton;
	public AudioClip	sndBuyZombie;
	public AudioClip	sndDiscover;
	public AudioClip	sndEvolve;
	public AudioClip	sndSwitchMap;
	public AudioClip	sndTapTomb;
	
	private int[] numberUnits = new int[3];
	
	public bool showingTutorial = true;
	
	private float tombSpawnTime = SCR_Config.TOMB_SPAWN_INTERVAL;
	
	private Transform selectedZombie = null;
	private float offsetX = 0;
	private float offsetY = 0;
	
	private int currentMap = 0;
	private int nextMap = 0;
	
	private bool switchingMap = false;

	[System.NonSerialized] public TutorialPhase tutorialPhase = TutorialPhase.OPEN_TOMB;
	private Transform tutorialZombie1 = null;
	private Transform tutorialZombie2 = null;
	
	private SCR_ZombieShop scrZombieShop = null;
	private SCR_UpgradeShop scrUpgradeShop = null;
	
	[System.NonSerialized] public bool pendingDiscover = false;
	[System.NonSerialized] public int pendingIndex = 0;
	
	private int totalProductionRate = 0;
	private float productionTime = 0;
	
	private BannerView bannerView;
	
	public void Awake() {
		instance = this;
	}
	
	public void Start() {
		//SCR_Profile.Reset();
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
		
		SCR_Profile.Load();
		
		if (!SCR_Profile.finishedTutorial) {
			SCR_Profile.Reset();
		}
		
		txtBrain.text = FormatNumber(SCR_Profile.brain);
		
		for (int i = 0; i < SCR_Profile.NUMBER_ZOMBIES; i++) {
			SpawnZombie(i, SCR_Profile.numberZombies[i]);
		}
		
		for (int i = 0; i < SCR_Profile.numberTombs; i++) {
			SpawnTomb();
		}
		
		showingTutorial = !SCR_Profile.finishedTutorial;

		backgrounds[0].SetActive(true);

		for (int i = 1; i < backgrounds.Length; i++) {
			backgrounds[i].SetActive(false);
		}
		
		grpAreaIsFull.SetActive(false);
		grpPerfect.SetActive(false);
		
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
		cvsCoin.SetActive(true);
		cvsDiscover.SetActive(false);
		
		scrZombieShop = cvsGameplay.transform.Find("ZombieShop").GetComponent<SCR_ZombieShop>();
		scrUpgradeShop = cvsUpgrade.transform.Find("UpgradeShop").GetComponent<SCR_UpgradeShop>();
		
		UpdateTotalProductionRate();
		
		#if UNITY_ANDROID
			string appId = "ca-app-pub-0081066185741622~5075136082";
		#elif UNITY_IPHONE
			string appId = "ca-app-pub-0081066185741622~6955193968";
		#else
			string appId = "unexpected_platform";
		#endif

		MobileAds.Initialize(appId);
		
		RequestBanner();
		
		SetBannerTop();
	}
	
	private void RequestBanner() {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-0081066185741622/3194669084";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-0081066185741622/6572050581";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
		
		AdRequest request = new AdRequest.Builder()
		.AddTestDevice("70A04A3097E1E7BAAF564A4F70E42D77")
		.Build();
		
        bannerView.LoadAd(request);
    }
	
	private void SetBannerTop() {
		if (Screen.dpi != 0) {
			bannerView.SetPosition(0, Mathf.RoundToInt(Screen.height * BANNER_GAMEPLAY_Y / (Screen.dpi / 160)));
		}
		else {
			bannerView.SetPosition(AdPosition.Top);
		}
	}
	
	public void Update() {
		if (!cvsDiscover.activeSelf
		&&  !cvsUpgrade.activeSelf
		&&  !cvsCollection.activeSelf
		&&  !scrZombieShop.gameObject.activeSelf) {
			if (Input.GetMouseButtonDown(0)) {
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D bestHit = FindBestHit(pos);
				if (bestHit.transform != null) {
					if (bestHit.transform.parent != null) {
						SCR_Tomb scrTomb = bestHit.transform.parent.GetComponent<SCR_Tomb>();
						if (scrTomb != null) {
							GameObject zombie = SpawnZombie(0);	// numberUnits[0]++ in SpawnZombie(0)
							
							numberUnits[0]--;
							
							SCR_Profile.numberZombies[0]++;
							SCR_Profile.SaveNumberZombies();
							
							SCR_Profile.numberTombs--;
							SCR_Profile.SaveNumberTombs();
							
							UpdateTotalProductionRate();
							
							zombie.transform.position = bestHit.transform.parent.position;
							Destroy(bestHit.transform.parent.gameObject);

							if (showingTutorial) {
								if (tutorialPhase == TutorialPhase.OPEN_TOMB) {
									tutorialZombie1 = zombie.transform;
									ShowTutorial(TutorialPhase.OPEN_ZOMBIE_SHOP);
								}
							}
							
							source.PlayOneShot(sndTapTomb);
						}
					}
					
					if (!showingTutorial || tutorialPhase == TutorialPhase.EVOLVE_ZOMBIE) {
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
							if (selectedZombie.GetComponent<SCR_Zombie>().type == scrZombie.type) {
								if (scrZombie.type != SCR_Zombie.LAST_TYPE) {
									FuseZombie(selectedZombie.gameObject, scrZombie.gameObject);
								}
								else {
									grpPerfect.SetActive(true);
								}
							}
						}
					}
					
					selectedZombie.GetComponent<SCR_Zombie>().state = ZombieState.AUTO_MOVE;
					
					selectedZombie.GetComponent<Collider2D>().enabled = true;
					selectedZombie = null;
				}
			}
		}

		if (!showingTutorial) {
			tombSpawnTime += Time.deltaTime;
			if (tombSpawnTime >= SCR_Config.TOMB_SPAWN_INTERVAL && numberUnits[0] < SCR_Config.MAX_NUMBER_ZOMBIES) {
				SpawnTomb();
				tombSpawnTime = 0;
				
				SCR_Profile.numberTombs++;
				SCR_Profile.SaveNumberTombs();
			}
			
			productionTime += Time.deltaTime;
			if (productionTime >= 1) {
				IncreaseBrain(totalProductionRate);
				productionTime = 0;
			}
		}		
	}
	
	public void UpdateTotalProductionRate() {
		totalProductionRate = 0;
		
		for (int i = 0; i < SCR_Profile.NUMBER_ZOMBIES; i++) {
			int productionRate = SCR_Config.GetProductionRate(i) * SCR_Profile.numberZombies[i];
			totalProductionRate += productionRate;
		}
		
		txtTotalProductionRate.text = FormatNumber(totalProductionRate) + " brains/s";
	}
	
	public void SpawnTomb() {
		float x = Random.Range(GARDEN_LEFT, GARDEN_RIGHT);
		float y = Random.Range(GARDEN_BOTTOM, GARDEN_TOP);
		float z = y;
		
		GameObject tomb = Instantiate(PFB_TOMB, backgrounds[0].transform);
		tomb.transform.localPosition = new Vector3(x, y, z);

		numberUnits[0]++;
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
	
	public GameObject SpawnZombie(int index) {
		float x = Random.Range(GARDEN_LEFT, GARDEN_RIGHT);
		float y = Random.Range(GARDEN_BOTTOM, GARDEN_TOP);
		float z = y;
		
		int map = GetMapFromZombieIndex(index);
		
		GameObject zombie = Instantiate(PFB_ZOMBIES[index], backgrounds[map].transform);
		zombie.transform.position = new Vector3(x, y, z);
		
		numberUnits[map]++;
		
		return zombie;
	}
	
	public void SpawnZombie(int index, int count) {
		for (int i = 0; i < count; i++) {
			SpawnZombie(index);
		}
	}
	
	public void FuseZombie(GameObject zombie1, GameObject zombie2) {
		int originalIndex = (int)zombie1.GetComponent<SCR_Zombie>().type;
		int zombieIndex = originalIndex + 1;

		int originalMap = GetMapFromZombieIndex(originalIndex);		
		int map = GetMapFromZombieIndex(zombieIndex);
		
		if (map == originalMap || numberUnits[map] < SCR_Config.MAX_NUMBER_ZOMBIES) {
			Vector3 position = (zombie1.transform.position + zombie2.transform.position) * 0.5f;
			
			if (map != originalMap) {
				GameObject movedZombie = Instantiate(PFB_ZOMBIES[zombieIndex], backgrounds[originalMap].transform);
				movedZombie.transform.position = new Vector3(0, -movedZombie.GetComponent<BoxCollider2D>().offset.y, 0);
				movedZombie.GetComponent<SCR_Zombie>().SwitchMapEffect();
				
				if (zombieIndex > SCR_Profile.zombieUnlocked) {
					pendingDiscover = true;
					pendingIndex = zombieIndex;
				}
			}
			
			GameObject zombie = Instantiate(PFB_ZOMBIES[zombieIndex], backgrounds[map].transform);
			zombie.transform.position = position;
			Instantiate(PFB_FUSE_EFFECT, position, PFB_FUSE_EFFECT.transform.rotation);
			
			Destroy(zombie1);
			Destroy(zombie2);

			numberUnits[map]++;
			numberUnits[originalMap] -= 2;
			
			SCR_Profile.numberZombies[zombieIndex]++;
			SCR_Profile.numberZombies[originalIndex] -= 2;
			SCR_Profile.SaveNumberZombies();
			
			UpdateTotalProductionRate();
			
			if (showingTutorial) {
				if (tutorialPhase == TutorialPhase.EVOLVE_ZOMBIE) {
					hand.SetActive(false);
					showingTutorial = false;
					SCR_Profile.finishedTutorial = true;
					SCR_Profile.SaveTutorial();
				}
			}
			
			if (zombieIndex > SCR_Profile.zombieUnlocked) {
				if (!pendingDiscover) {
					cvsDiscover.GetComponent<SCR_DiscoverZombie>().ShowNewZombie(zombieIndex);
					cvsDiscover.SetActive(true);
					source.PlayOneShot(sndDiscover);
				}
				
				SCR_Profile.zombieUnlocked = zombieIndex;
				SCR_Profile.SaveZombieUnlocked();
				
				scrZombieShop.RefreshUnlocked();
				scrUpgradeShop.RefreshUnlocked();
			}
			
			source.PlayOneShot(sndEvolve);
		}
		else {
			grpAreaIsFull.GetComponent<SCR_AutoFade>().txtTitle.text = "NEXT AREA IS FULL!";
			grpAreaIsFull.SetActive(true);
		}
	}
	
	public void IncreaseBrain(int amount) {
		SCR_Profile.brain += amount;
		txtBrain.text = FormatNumber(SCR_Profile.brain);
		SCR_Profile.SaveBrain();
		scrZombieShop.UpdateBrain();
		scrUpgradeShop.UpdateBrain();
	}

	public void DecreaseBrain(int amount) {
		SCR_Profile.brain -= amount;
		txtBrain.text = FormatNumber(SCR_Profile.brain);
		SCR_Profile.SaveBrain();
		scrZombieShop.UpdateBrain();
		scrUpgradeShop.UpdateBrain();
	}

	public void SwitchMap(int map) {
		if (!showingTutorial && !cvsDiscover.activeSelf && !switchingMap && map != currentMap) {
			source.PlayOneShot(sndSwitchMap);
			
			switchingMap = true;
			nextMap = map;
			
			if (currentMap < map) {
				DisappearShrinkMap(currentMap);
			}

			if (currentMap > map) {
				DisappearEnlargeMap(currentMap);
			}
		}
		else {
			source.PlayOneShot(sndButton);
		}
	}

	public void DisappearShrinkMap(int map) {
		iTween.ScaleTo(backgrounds[currentMap], iTween.Hash("x", 0.01f, "y", 0.01f, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteDisappearShrinkMap", "oncompletetarget", gameObject));
	}

	public void AppearShrinkMap(int map) {
		backgrounds[map].SetActive(true);
		backgrounds[map].transform.localScale = new Vector3(10, 10, 1);
		iTween.ScaleTo(backgrounds[map], iTween.Hash("x", 1, "y", 1, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteSwitchMap", "oncompletetarget", gameObject));
	}

	public void CompleteDisappearShrinkMap() {
		backgrounds[currentMap].SetActive(false);
		AppearShrinkMap(nextMap);
	}

	public void DisappearEnlargeMap(int map) {
		iTween.ScaleTo(backgrounds[currentMap], iTween.Hash("x", 10, "y", 10, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteDisappearEnlargeMap", "oncompletetarget", gameObject));
	}

	public void AppearEnlargeMap(int map) {
		backgrounds[map].SetActive(true);
		backgrounds[map].transform.localScale = new Vector3(0.01f, 0.01f, 1);
		iTween.ScaleTo(backgrounds[map], iTween.Hash("x", 1, "y", 1, "time", 0.5f, "easetype", "easeInOutSine", "oncomplete", "CompleteSwitchMap", "oncompletetarget", gameObject));
	}

	public void CompleteDisappearEnlargeMap() {
		backgrounds[currentMap].SetActive(false);
		AppearEnlargeMap(nextMap);
	}

	public void CompleteSwitchMap() {
		currentMap = nextMap;
		switchingMap = false;
		
		if (pendingDiscover) {
			cvsDiscover.GetComponent<SCR_DiscoverZombie>().ShowNewZombie(pendingIndex);
			cvsDiscover.SetActive(true);
			pendingDiscover = false;
			source.PlayOneShot(sndDiscover);
		}
	}

	public void OpenZombieShop() {
		if (!showingTutorial || tutorialPhase == TutorialPhase.OPEN_ZOMBIE_SHOP) {
			if (!cvsDiscover.activeSelf) {
				zombieShop.SetActive(true);
				bannerView.SetPosition(AdPosition.Bottom);
				
				if (showingTutorial) {
					if (tutorialPhase == TutorialPhase.OPEN_ZOMBIE_SHOP) {
						zombieShop.GetComponent<ScrollRect>().vertical = false;
						ShowTutorial(TutorialPhase.BUY_ZOMBIE);
					}
				}
			}
		}
	}

	public void CloseZombieShop() {
		if (!showingTutorial || tutorialPhase == TutorialPhase.CLOSE_ZOMBIE_SHOP) {
			zombieShop.SetActive(false);
			SetBannerTop();
			
			if (showingTutorial) {
				if (tutorialPhase == TutorialPhase.CLOSE_ZOMBIE_SHOP) {
					zombieShop.GetComponent<ScrollRect>().vertical = true;
					ShowTutorial(TutorialPhase.EVOLVE_ZOMBIE);
				}
			}
		}
	}

	public void OpenUpgrade() {
		if (!showingTutorial && !cvsDiscover.activeSelf) {
			cvsGameplay.SetActive(false);
			cvsUpgrade.SetActive(true);
			
			bannerView.SetPosition(AdPosition.Bottom);
		}
	}

	public void CloseUpgrade() {
		cvsUpgrade.SetActive(false);
		cvsGameplay.SetActive(true);
		
		SetBannerTop();
	}

	public void OpenCollection() {
		if (!showingTutorial && !cvsDiscover.activeSelf) {
			cvsGameplay.SetActive(false);
			cvsCollection.SetActive(true);
			
			cvsCoin.SetActive(false);
			bannerView.SetPosition(AdPosition.Bottom);
		}
	}

	public void CloseCollection() {
		cvsCollection.SetActive(false);
		cvsGameplay.SetActive(true);
		
		cvsCoin.SetActive(true);
		SetBannerTop();
	}

	public void BuyZombie(int index) {
		if (!showingTutorial || tutorialPhase == TutorialPhase.BUY_ZOMBIE) {
			int map = GetMapFromZombieIndex(index);

			if (numberUnits[map] < SCR_Config.MAX_NUMBER_ZOMBIES && SCR_Profile.brain >= SCR_Config.ZOMBIE_INFO[index].price) {
				GameObject zombie = SpawnZombie(index);
				SCR_Profile.numberZombies[index]++;
				SCR_Profile.SaveNumberZombies();
				
				UpdateTotalProductionRate();

				DecreaseBrain(SCR_Config.ZOMBIE_INFO[index].price);
				
				if (showingTutorial) {
					if (tutorialPhase == TutorialPhase.BUY_ZOMBIE) {
						if (index == 0) {
							tutorialZombie2 = zombie.transform;
							ShowTutorial(TutorialPhase.CLOSE_ZOMBIE_SHOP);
						}
					}
				}
				
				source.PlayOneShot(sndBuyZombie);
			}
			else {
				grpAreaIsFull.GetComponent<SCR_AutoFade>().txtTitle.text = "AREA IS FULL!";
				grpAreaIsFull.SetActive(true);
				
				source.PlayOneShot(sndButton);
			}
		}
	}
	
	public static int GetMapFromZombieIndex(int zombieIndex) {
		int map = 0;
		
		if (zombieIndex <= 4) {
			map = 0;
		}
		else if (zombieIndex <= 9) {
			map = 1;
		}
		else if (zombieIndex <= 14) {
			map = 2;
		}
		
		return map;
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

			numberUnits[0]++;
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
		if (tutorialZombie1 != null && tutorialZombie2 != null) {
			float x = (tutorialZombie2.position.x - tutorialZombie1.position.x) * t + tutorialZombie1.position.x;
			float y = (tutorialZombie2.position.y - tutorialZombie1.position.y) * t + tutorialZombie1.position.y;
			
			x = x * 100 + HAND_ZOMBIE_OFFSET_X;
			y = y * 100 + HAND_ZOMBIE_OFFSET_Y;
			
			hand.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
		}
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
	
	public static string FormatNumber(int n)
	{
		if (n < 1000)
			return n.ToString();

		if (n < 10000)
			return string.Format("{0:#,.##}K", n - 5);

		if (n < 100000)
			return string.Format("{0:#,.#}K", n - 50);

		if (n < 1000000)
			return string.Format("{0:#,.}K", n - 500);

		if (n < 10000000)
			return string.Format("{0:#,,.##}M", n - 5000);

		if (n < 100000000)
			return string.Format("{0:#,,.#}M", n - 50000);

		if (n < 1000000000)
			return string.Format("{0:#,,.}M", n - 500000);

		return string.Format("{0:#,,,.##}B", n - 5000000);
	}
}

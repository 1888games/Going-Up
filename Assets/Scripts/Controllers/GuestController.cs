using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GuestController : MonoBehaviourSingleton<GuestController> {

    public List<Guest> guests;
    public GameObject guestPrefab;

    public List<GameObject> guestGameObjects;

    public Dictionary<Guest, GameObject> guestToGameObjectMap;
    public Dictionary<GameObject, Guest> gameObjectToGuestMap;

    public Transform worldTransform;

    public List<Vector2> liftSlotPositions;

    public List<Image> floorCountImages;
    public List<TextMeshProUGUI> floorCountText;

    public Dictionary<int, int> guestCount;

	public float maxSpawnGap = 10f;
    public float spawnGap = 10f;
    public float spawnTimer = 0f;
	public float spawnReduce = 0.05f;
	public float minSpawnGap = 5f;

    public bool gameActive = false;

	public TextMeshProUGUI currentScoreText;
	public TextMeshProUGUI bestScoreText;

	public int currentScore;
	public int highScore;
	public int lives;

	public List<GameObject> hearts;

	public bool restarted = false;

	public GameObject tutorial;
	public TextMeshProUGUI tutorialText;

	public GameObject gameOverScreen;

	public bool clickTextDone = false;
	public bool liftTextDone = false;
	public bool closeTextDone = false;
	
	

    // Use this for initialization
    void Start() {


		if (PlayerPrefs.HasKey ("High") == false) {

			PlayerPrefs.SetInt ("High", 0);

		}

		
		 NewGame (false);
		  
        for(int i = 0; i < 10; i++) {

            guestCount.Add(i, 0);
            UpdateLeftPanel(i);
        }
        
      



        liftSlotPositions.Add(new Vector2(-0.2f, 2.85f));
        liftSlotPositions.Add(new Vector2(0.2f, 2.85f));
        liftSlotPositions.Add(new Vector2(-0.2f, 2.5f));
        liftSlotPositions.Add(new Vector2(0.2f, 2.5f));
        liftSlotPositions.Add(new Vector2(-0.2f, 2.15f));
        liftSlotPositions.Add(new Vector2(0.2f, 2.15f));

		tutorial.SetActive (true);

    }



	public void Restart () {

		gameOverScreen.SetActive (false);
		NewGame ();



	}
	
	
	void NewGame (bool restarted = true) {

		if (guests != null) {

			for (int i = guests.Count - 1; i >= 0; i--) {
				
				guestToGameObjectMap.Remove (guests [i]);
				guests [i] = null;
				guests.RemoveAt (i);
				

			}

			foreach (GameObject guestGO in guestGameObjects) {
				
				gameObjectToGuestMap.Remove (guestGO);
				SimplePool.Despawn (guestGO);
			
			}

		}
		
		guests = new List<Guest>();
        guestGameObjects = new List<GameObject>();

        guestToGameObjectMap = new Dictionary<Guest, GameObject>();
        gameObjectToGuestMap = new Dictionary<GameObject, Guest>();

        guestCount = new Dictionary<int, int>();

		spawnGap = maxSpawnGap;

		lives = 3;
		currentScore = 0;
		highScore = PlayerPrefs.GetInt ("High");


		UpdateScoreText ();

		hearts [0].SetActive (true);
		hearts [1].SetActive (true);
		hearts [2].SetActive (true);

		if (restarted) {

			HotelController.Instance.floorBeingViewed = 0;
			HotelController.Instance.liftBeingControlled = 0;
			
			  
	        for(int i = 0; i < 10; i++) {
	
	            guestCount[i] = 0;
	            UpdateLeftPanel(i);
	        }

			for (int i = 0; i < 3; i++) {

				HotelController.Instance.hotel.lifts [i].currentFloor = 0;
				HotelController.Instance.hotel.lifts [i].targetFloor = 0;
				HotelController.Instance.hotel.lifts [i].distanceToTargetFloor = 0f;
				HotelController.Instance.hotel.lifts [i].doorsOpen = false;
				HotelController.Instance.hotel.lifts [i].doorsClosed = true;
				HotelController.Instance.hotel.lifts [i].doorsOpening = false;
				HotelController.Instance.hotel.lifts [i].doorsClosing = false;
				HotelController.Instance.hotel.lifts [i].accelerating = false;
				HotelController.Instance.hotel.lifts [i].decelerating = false;
				HotelController.Instance.hotel.lifts [i].goingUp = false;
				HotelController.Instance.hotel.lifts [i].goingDown = false;
				HotelController.Instance.hotel.lifts [i].liftOpenTimer = 0f;

				LiftController.Instance.SetLiftPositionFromFloor (HotelController.Instance.hotel.lifts [i], 0);
				

				for (int y = 0; y < 10; y++) {

					HotelController.Instance.hotel.lifts [i].liftButtonsPressed [y] = false;
				}
				
				for (int z = 0; z < 6; z++) {

					HotelController.Instance.hotel.lifts [i].slots [z] = null;
				}

			


			}
			
		HotelController.Instance.ShowCorrectFloor ();
		SpawnGuest(0);
		HotelController.Instance.hotel.lifts[0].doorsOpening = true;

			
       	 gameActive = true;

		}

		


	}


	void Initialise () {

		


	}

	void UpdateScoreText () {

		currentScoreText.text = "Score: " + string.Format ("{0:n0}", currentScore);
		bestScoreText.text = "High: " + string.Format ("{0:n0}", highScore);

	}


	void AddToScore (int score) {

		currentScore += score;

		if (currentScore > highScore) {

			highScore = currentScore;
	
		}

		UpdateScoreText ();

	}
	
	
    // Update is called once per frame
    void Update() {

        

        if (gameActive) {

            foreach (Guest guest in guests) {

                guest.UpdateGuest();
            }

            CheckClickOnGuest();

            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnGap || guests.Count == 0) {

                SpawnGuest();
                spawnTimer = 0f;
            }

         
        }
       
    }


    void UpdateLeftPanel(int floor) {

        floorCountText[floor].text = guestCount[floor].ToString();

        float H = (float)Mathf.Max(0, 5 - guestCount[floor]) / 5f * 120f / 360f;
        floorCountImages[floor].color = Color.HSVToRGB(H, 1f, 0.8f);
        
    }


    public void UpdatePatienceBar(Guest guest) {

        if (guest.hud != null) {


            guest.hud.patienceBar.sizeDelta = new Vector3(0.46f * guest.patience, 0.16f);

            guest.hud.patienceBarImage.color = Color.HSVToRGB((150f * guest.patience) / 360f, 0.85f, 0.85f);
        }


    }

    void CheckClickOnGuest() {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f)) {

                GameObject go = hit.transform.gameObject;



                if (go.name == "Guest") {

                    HandleClickOnGuest(go);
                }

                if (go.transform.parent.name == "Guest") {

                    HandleClickOnGuest(go.transform.parent.gameObject);
                }

            }

        }



    }


    void HandleClickOnGuest(GameObject guestGO) {

        Lift lift = HotelController.Instance.GetCurrentLift();

        Guest guest = gameObjectToGuestMap[guestGO];

        if (guest.guestReadyForLift && lift.currentFloor == guest.currentFloor && lift.doorsClosed == false && guest.lift == null) {


            Debug.Log("CLICKED ON GUEST: " + guest.currentFloor + " " + guest.patience);

			if (clickTextDone == false) {
				clickTextDone = true;
				tutorialText.text = "SELECT GUEST'S REQUESTED FLOOR";
			}

            for (int i = 0; i < 6; i++) {

                if (lift.slots[i] == null) {

                    lift.slots[i] = guest;
                    SendGuestToLift(guest, lift, i);
                    return;
                }

            }
        }


    }

  
    public void SendGuestToLift(Guest guest, Lift lift, int slotID) {

        GameObject guestGO = guestToGameObjectMap[guest];
        GameObject liftGO = LiftController.Instance.liftToGameObjectMap[lift];

        guestGO.transform.DOMove(new Vector3(-2f + liftGO.transform.position.x, guestGO.transform.position.y, 1.5f), 1f).SetEase(Ease.Linear)
            .OnComplete(() => WalkIntoLift(guestGO, guest, lift, slotID, liftGO));

        
        if (lift.doorsClosing) {

            lift.doorsClosing = false;
            lift.doorsOpening = true;

        
        }

		lift.liftOpenTimer = 0f;

    }


    public void WalkIntoLift(GameObject guestGO, Guest guest, Lift lift, int slotID, GameObject liftGO) {


        guestGO.transform.DOMove(new Vector3(-2f + liftGO.transform.position.x + liftSlotPositions[slotID].x, guestGO.transform.position.y, liftSlotPositions[slotID].y), 0.5f)
            .OnComplete(() => GuestArrivedInLift(guestGO, guest, lift));

    }

    public void GetOutOfLift(Guest guest, Lift lift, int slotID) {

        GameObject guestGO = guestToGameObjectMap[guest];
        GameObject liftGO = LiftController.Instance.liftToGameObjectMap[lift];
        guest.reachedDestination = true;
        guestGO.GetComponent<FaceCanvas>().hud.gameObject.SetActive(false);

        guestGO.transform.DOMove(new Vector3(-2f + liftGO.transform.position.x, guestGO.transform.position.y, 1.5f), 0.5f)
        .OnComplete(() => WalkToExit(guestGO, guest, lift, slotID, lift.currentFloor));

    }
    
    public void WalkToExit(GameObject guestGO, Guest guest, Lift lift, int slotID, int newFloor) {


        guest.currentFloor = newFloor;

        lift.slots[slotID] = null;
        guest.lift = null;



        guestGO.transform.DOMove(new Vector3(4.5f, (float)lift.currentFloor, 0f), 2f)
       .OnComplete(() => DestroyGuest(guestGO, guest));

		AddToScore (Mathf.RoundToInt (guest.patience * 100f));
        

    }

	public void TakeTheStairs (Guest guest) {
		
		GameObject guestGO = guestToGameObjectMap[guest];

		LoseLife ();

		if (guest.lift == null) {
			
			 guestGO.transform.DOMove(new Vector3(-4.5f, (float)guest.currentFloor, 0f), 2f)
       		.OnComplete(() => DestroyGuest(guestGO, guest));

		}


	}

	void LoseLife () {

		lives = lives - 1;

		if (lives == 0) {

			GameOver ();

		}

		hearts [lives].SetActive (false);


	}
	
	void GameOver() {

		gameActive = false;

		gameOverScreen.SetActive (true);

		PlayerPrefs.SetInt ("High", highScore);

	}

    public void DestroyGuest(GameObject guestGO, Guest guest) {

        guests.Remove(guest);
        guestGameObjects.Remove(guestGO);
        guestToGameObjectMap.Remove(guest);
        gameObjectToGuestMap.Remove(guestGO);

        SimplePool.Despawn(guestGO);
    }


    public void GuestArrivedInLift(GameObject guestGO, Guest guest, Lift lift) {

        guest.lift = lift;
        guestGO.transform.eulerAngles = Vector3.zero;

        guestCount[guest.currentFloor]--;
        UpdateLeftPanel(guest.currentFloor);
    }

    public void CheckOnVisibleFloor(Guest guest) {

        GameObject guestGO = guestToGameObjectMap[guest];

        //Debug.Log(guest.currentFloor + " vs " + HotelController.Instance.floorBeingViewed);

        if (guest.currentFloor == HotelController.Instance.floorBeingViewed || guest.lift != null) {

            guestGO.SetActive(true);
        }

        else { 

            guestGO.SetActive(false);
        }
    }

    public void SpawnGuest(int fixFloor = 99) {

        
        int currentFloor = Random.Range(0, HotelController.Instance.hotel.floors.Count);

 
        int targetFloor = currentFloor;
        float maxWaitingTime = Random.Range(50f, 120f);
        
       // maxWaitingTime = Random.Range(1, 2f);
        
        
        while (targetFloor == currentFloor) {

            targetFloor = Random.Range(0, HotelController.Instance.hotel.floors.Count);

        }

        if (fixFloor != 99) {
            currentFloor = fixFloor;
            targetFloor = 1;
        }


        Guest guest = new Guest(currentFloor, targetFloor, maxWaitingTime);

        GameObject guestGO = SimplePool.Spawn(guestPrefab, Vector3.zero, Quaternion.identity);


        guestGO.transform.SetParent(worldTransform);
        guestGO.transform.position = new Vector3(4.5f, (float)currentFloor, 0f);
        guestGO.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        guestGO.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        guestGO.GetComponent<FaceCanvas>().hud.gameObject.SetActive(true);
        guestGO.SetActive(true);

        guests.Add(guest);
        guestGameObjects.Add(guestGO);

        guestToGameObjectMap.Add(guest, guestGO);
        gameObjectToGuestMap.Add(guestGO, guest);

        guestGO.transform.DOMove(new Vector3(Random.Range(-1.8f, 1.2f), guestGO.transform.position.y, Random.Range(-0.5f, 1.5f)), Random.Range(2f, 3f))
            .OnComplete(() => GuestReadyForLift(guest, guestGO));

        guestGO.name = "Guest";

        FaceCanvas hud = guestGO.GetComponent<FaceCanvas>();

        hud.targetFloorText.text = HotelController.Instance.GetFloor(targetFloor).shortName;
        guest.hud = hud;
        UpdatePatienceBar(guest);

        guestCount[currentFloor]++;
        UpdateLeftPanel(currentFloor);

		spawnGap = Mathf.Max (minSpawnGap, spawnGap - spawnReduce);


    }


    public void GuestReadyForLift(Guest guest, GameObject guestGO) {

        guestGO.transform.eulerAngles = new Vector3(0f, Random.Range(120f, 240f), 0f);
        guest.guestReadyForLift = true;


    }


   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LiftController : MonoBehaviourSingleton<LiftController> {

	public List<GameObject> liftGameObjects;

	public Dictionary<GameObject, Lift> gameObjectToLiftMap;
	public Dictionary<Lift, GameObject> liftToGameObjectMap;

	public TextMeshProUGUI liftSelectedText;
	public bool liftsReady = false;

    float doorOpenValue = 0.45f;
    float doorClosedValue = 0.175f;
    
    float backDoorOpenValue = 0.9f;
    float backDoorClosedValue = 0.35f;

    public Dictionary<int, TextMeshProUGUI> liftFloorNumbers;
    public Dictionary<int, GameObject> liftUpSigns;
    public Dictionary<int, GameObject> liftDownSigns;

    public List<Image> liftButtonLights;

    public float frontDoorsSpeed = 0.01f;
    public float backDoorsSpeed = 0.0025f;

	// Use this for initialization
	void Start () {

		gameObjectToLiftMap = new Dictionary<GameObject, Lift> ();
		liftToGameObjectMap = new Dictionary<Lift, GameObject> ();

		
	}
	
	// Update is called once per frame
	void Update () {

		if (liftsReady && GuestController.Instance.gameActive) {

			foreach (Lift lift in liftToGameObjectMap.Keys) {

				lift.ManageLift ();

			}

            CheckClickOnLift();
			CheckSpace ();
			CheckNumbers ();

		}
		
	}


	void CheckSpace () {

		if (Input.GetKeyDown (KeyCode.Space)) {

			Lift lift = HotelController.Instance.GetCurrentLift ();
			ToggleLiftDoors (lift);
		}
		
			
			
	}


	void CheckNumbers () {

		
		 for ( int i = 0; i < 10; ++i )
		 {
		     if ( Input.GetKeyDown( "" + i ) )
		     {
		     
		       Lift lift = HotelController.Instance.GetCurrentLift();
			       
	            if (lift.currentFloor != i) {
	                lift.liftButtonsPressed[i] = !lift.liftButtonsPressed[i];
	                UpdateLiftButtonLight(lift, i);
	                
	                
					if (GuestController.Instance.liftTextDone == false) {
						GuestController.Instance.liftTextDone = true;
						GuestController.Instance.tutorialText.text = "CLOSE LIFT DOOR TO SEND TO REQUESTED FLOORS";
					}
	            }

		         
		     }
		 }

		if (Input.GetKeyDown (KeyCode.G)) {
		
			Lift lift = HotelController.Instance.GetCurrentLift();
		       
            if (lift.currentFloor != 0) {
                lift.liftButtonsPressed[0] = !lift.liftButtonsPressed[0];
                UpdateLiftButtonLight(lift, 0);
                
                if (GuestController.Instance.liftTextDone == false) {
						GuestController.Instance.liftTextDone = true;
						GuestController.Instance.tutorialText.text = "CLOSE LIFT DOOR TO SEND TO REQUESTED FLOORS";
				}
					
            }


		}


	}

    void CheckClickOnLift() {

		if (GuestController.Instance.gameActive) {

			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {

				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100f)) {

					GameObject go = hit.transform.gameObject;


					if (go.tag == "Lift") {

						HandleClickOnLift (go.transform.parent.parent.gameObject, Input.GetMouseButtonDown (0));
					}


				}

			}

		}



    }

    void HandleClickOnLift(GameObject go, bool leftButton) {

     
        Lift lift = gameObjectToLiftMap[go];

        if (leftButton) {

            HotelController.Instance.liftBeingControlled = lift.id;
            HotelController.Instance.selectionCylinder.SetCylinder(lift.id);
            LiftController.Instance.liftSelectedText.text = "Lift " + (HotelController.Instance.liftBeingControlled + 1).ToString();
            LiftController.Instance.UpdateLiftButtonLights();

        }
        else {

            Debug.Log("Right click on lift " + lift.id);


            if (lift.doorsOpening || lift.doorsOpen) {
                lift.doorsClosing = true;
                lift.doorsOpening = false;
                lift.doorsOpen = false;
                 SoundController.Instance.Doors ();
               
            }

            else {

                lift.doorsOpening = true;
                lift.doorsClosing = false;
                lift.doorsClosed = false;
                 SoundController.Instance.Doors ();
            }
        }

    }
    
    
    void ToggleLiftDoors(Lift lift) {
    
     if (lift.doorsOpening || lift.doorsOpen) {
                lift.doorsClosing = true;
                lift.doorsOpening = false;
                lift.doorsOpen = false;
			SoundController.Instance.Doors ();
			
			 if (GuestController.Instance.closeTextDone == false && GuestController.Instance.clickTextDone && GuestController.Instance.liftTextDone) {
						GuestController.Instance.closeTextDone = true;
						GuestController.Instance.tutorial.SetActive (false);
				}
               
        }

        else {

            lift.doorsOpening = true;
            lift.doorsClosing = false;
            lift.doorsClosed = false;
            SoundController.Instance.Doors ();
            
             
					
        }

	}
    
    
    
    public void OnClickLiftButton(string button) {

		if (GuestController.Instance.gameActive) {

			Lift lift = HotelController.Instance.GetCurrentLift ();

			if (button == "Open" && lift.distanceToTargetFloor == 0f) {
				lift.doorsOpening = true;
				lift.doorsClosing = false;
				 SoundController.Instance.Doors ();
				return;
			}

			if (button == "Close" && lift.doorsClosed == false) {

				lift.doorsClosing = true;
				lift.doorsOpening = false;
				 SoundController.Instance.Doors ();
				 
				 if (GuestController.Instance.closeTextDone == false && GuestController.Instance.clickTextDone && GuestController.Instance.liftTextDone) {
						GuestController.Instance.closeTextDone = true;
						GuestController.Instance.tutorial.SetActive (false);
				}
				 
				 
				return;
			}

			if (button != "Close" && button != "Open") {

				int buttonPressed = Convert.ToInt32 (button);

				if (lift.currentFloor != buttonPressed) {
					lift.liftButtonsPressed [buttonPressed] = !lift.liftButtonsPressed [buttonPressed];
					UpdateLiftButtonLight (lift, buttonPressed);
					
					if (GuestController.Instance.liftTextDone == false && GuestController.Instance.clickTextDone) {
						GuestController.Instance.liftTextDone = true;
						GuestController.Instance.tutorialText.text = "CLOSE LIFT DOOR TO SEND TO REQUESTED FLOORS";
					}
				}
				
				

				Debug.Log ("Button pressed: " + button);

			}


		}



       
        
	}
	
	
	
	
	public void TestLift (int liftID, int floorID) {

		gameObjectToLiftMap [liftGameObjects[liftID]].SetTargetFloor (floorID);

	}

	public List<Lift> CreateLifts (Hotel hotel) {

		List<Lift> lifts = new List<Lift> ();

		for (int i = 0; i < 3; i++) {

			Lift lift = new Lift (i, hotel);
			lifts.Add (lift);

			gameObjectToLiftMap.Add (liftGameObjects [i], lift);
			liftToGameObjectMap.Add (lift, liftGameObjects [i]);


        }

		liftsReady = true;

		return lifts;
		
	}

	public GameObject GetLiftGameObject (Lift lift) {
		
		return liftToGameObjectMap [lift];



	}
	
	
    public void OpenDoors(Lift lift) {
    
   		lift.liftOpenTimer = 0f;

		lift.doorsOpening = true;
		lift.doorsClosed = false;

        GameObject liftGo = GetLiftGameObject(lift);
        Transform liftTransform = GetLiftGameObject(lift).transform;

		FloorObject floorObj = HotelController.Instance.GetFloorObject (lift.currentFloor);

        Doors doors = liftGo.GetComponent<Doors>();

		Transform frontDoorLeft = floorObj.leftdoors [lift.id];
		Transform frontDoorRight = floorObj.rightDoors [lift.id];

        frontDoorLeft.localPosition = new Vector3(
        Mathf.Clamp(frontDoorLeft.localPosition.x - frontDoorsSpeed, -doorOpenValue, -doorClosedValue),
       	frontDoorLeft.localPosition.y, frontDoorLeft.localPosition.z);

        frontDoorRight.localPosition = new Vector3(
        Mathf.Clamp(frontDoorRight.localPosition.x + frontDoorsSpeed, doorClosedValue, doorOpenValue),
        frontDoorRight.localPosition.y, frontDoorRight.localPosition.z);

        doors.backDoorRight.localPosition = new Vector3(
        Mathf.Clamp(doors.backDoorRight.localPosition.x + backDoorsSpeed, backDoorClosedValue, backDoorOpenValue),
        doors.backDoorRight.localPosition.y, doors.backDoorRight.localPosition.z);

        doors.backDoorLeft.localPosition = new Vector3(
        Mathf.Clamp(doors.backDoorLeft.localPosition.x - backDoorsSpeed, -backDoorOpenValue, -backDoorClosedValue),
        doors.backDoorLeft.localPosition.y, doors.backDoorLeft.localPosition.z);
        
        if (doors.backDoorRight.localPosition.x >= backDoorOpenValue) {

            lift.doorsOpening = false;
            lift.doorsOpen = true;
            lift.doorsClosed = false;
            lift.doorsClosing = false;
			lift.liftOpenTimer = 0f;
	
        }


    }
    
    

    public void CloseDoors(Lift lift) {

        GameObject liftGo = GetLiftGameObject(lift);

        Transform liftTransform = GetLiftGameObject(lift).transform;
        FloorObject floorObj = HotelController.Instance.GetFloorObject (lift.currentFloor);
        
          Doors doors = liftGo.GetComponent<Doors>();
        
        
       Transform frontDoorLeft = floorObj.leftdoors [lift.id];
		Transform frontDoorRight = floorObj.rightDoors [lift.id];

        frontDoorLeft.localPosition = new Vector3(
        Mathf.Clamp(frontDoorLeft.localPosition.x + frontDoorsSpeed, -doorOpenValue, -doorClosedValue),
       	frontDoorLeft.localPosition.y, frontDoorLeft.localPosition.z);

        frontDoorRight.localPosition = new Vector3(
        Mathf.Clamp(frontDoorRight.localPosition.x - frontDoorsSpeed, doorClosedValue, doorOpenValue),
        frontDoorRight.localPosition.y, frontDoorRight.localPosition.z);

        doors.backDoorRight.localPosition = new Vector3(
        Mathf.Clamp(doors.backDoorRight.localPosition.x - backDoorsSpeed, backDoorClosedValue, backDoorOpenValue),
        doors.backDoorRight.localPosition.y, doors.backDoorRight.localPosition.z);

        doors.backDoorLeft.localPosition = new Vector3(
        Mathf.Clamp(doors.backDoorLeft.localPosition.x + backDoorsSpeed, -backDoorOpenValue, -backDoorClosedValue),
        doors.backDoorLeft.localPosition.y, doors.backDoorLeft.localPosition.z);
        
        if (doors.backDoorRight.localPosition.x <= backDoorClosedValue) {

			lift.doorsClosing = false;
			lift.doorsClosed = true;
			lift.doorsOpen = false;
            lift.doorsOpening = false;
            

        }

    }
    
	public void ArrivedOnFloor (Lift lift) {

		GameObject liftGo = GetLiftGameObject (lift);
		
		Transform liftTransform = GetLiftGameObject (lift).transform;
		
		liftTransform.localPosition = new Vector3 (liftTransform.localPosition.x,
			(float)lift.targetFloor, liftTransform.localPosition.z);

		Debug.Log ("OPEN DOORS ON LIFT " + lift.id);
		
		Display display = liftGo.GetComponent<Display> ();
		display.currentSpeed = 0f;
		display.distanceToTargetFloor = 0f;

        lift.doorsOpening = true;
        lift.doorsClosing = false;
        lift.doorsClosed = false;
        lift.doorsOpen = true;
		lift.liftOpenTimer = 0f;

        lift.liftButtonsPressed[lift.currentFloor] = false;
        UpdateLiftButtonLight(lift, lift.currentFloor);

        CheckWhetherGuestsGetOut(lift);

		SoundController.Instance.Ding ();
		


	}

    void CheckWhetherGuestsGetOut(Lift lift) {

        for(int i= 0; i < 6; i++) {

            if (lift.slots[i] != null) {

                Guest guest = lift.slots[i];

                if (guest.targetFloor == lift.currentFloor) {

                    GuestController.Instance.GetOutOfLift(guest, lift, i);
                }
            }
        }
    }

	public void SetLiftPositionFromFloor (Lift lift, int floor) {
	
		Transform liftTransform = GetLiftGameObject (lift).transform;

		liftTransform.localPosition = new Vector3 (liftTransform.localPosition.x,
		(float)floor, liftTransform.localPosition.z);



	}


    public void UpdateLiftButtonLight(Lift lift, int i) {

        float alpha = 0f;

        if (lift.liftButtonsPressed[i]) {

            alpha = 1f;

            liftButtonLights[i].color = new Color(1f, 0.9274f, 0.3066f, 1f);
        }
        else {

            liftButtonLights[i].color = new Color(1f, 0.9274f, 0.3066f, 0f);
        }
    }

    public void UpdateLiftButtonLights() {

        Lift lift = HotelController.Instance.GetCurrentLift();

        for(int i = 0; i < lift.hotel.floors.Count; i++) {

            UpdateLiftButtonLight(lift, i);

    
        }
    }


    public void UpdateLiftArrows(Lift lift) {


        for (int i = 0; i < HotelController.Instance.hotel.floors.Count; i++) {

            FloorObject floorObj = HotelController.Instance.GetFloorObject(i);
            floorObj.floorDisplays[lift.id].upArrow.SetActive(lift.goingUp);
            floorObj.floorDisplays[lift.id].downArrow.SetActive(lift.goingDown);

        }


    
    }


    public void UpdateFloorNumber(Lift lift) {

        Floor newFloor = HotelController.Instance.GetFloor(lift.currentFloor);

        for (int i = 0; i < HotelController.Instance.hotel.floors.Count; i++) {

            Floor f = HotelController.Instance.GetFloor(i);

            FloorObject floorObj = HotelController.Instance.GetFloorObject(i);
            floorObj.floorDisplays[lift.id].floorDisplaysTMP[0].text = newFloor.shortName;


        }
    }

    public void MoveLift (Lift lift, float moveBy, float distanceToTargetFloor) {
		
		GameObject liftGo = GetLiftGameObject (lift);

		Transform liftTransform = GetLiftGameObject (lift).transform;

        int floorNum = Mathf.RoundToInt(liftTransform.localPosition.y);

        if (floorNum != lift.currentFloor) {

           // Debug.Log(floorNum + " vs " + lift.currentFloor);

            lift.currentFloor = floorNum;

            UpdateFloorNumber(lift);


        }

        if (liftTransform.localPosition.y + moveBy > liftTransform.localPosition.y || lift.currentFloor == 0) {


            if (lift.goingUp == false) {

                lift.goingUp = true;
                lift.goingDown = false;
                UpdateLiftArrows(lift);

            }

			
		}
		
		if (liftTransform.localPosition.y + moveBy < liftTransform.localPosition.y || lift.currentFloor == lift.hotel.floors.Count - 1) {

            if (lift.goingDown == false) {

                lift.goingUp = false;
                lift.goingDown = true;
                UpdateLiftArrows(lift);

            }
        }

         
        
		liftTransform.localPosition = new Vector3 (liftTransform.localPosition.x,
			liftTransform.localPosition.y + moveBy, liftTransform.localPosition.z);

        for(int i = 0; i < 6; i++) {

            if (lift.slots[i] != null) {

                Guest guest = lift.slots[i];

				if (GuestController.Instance.guestToGameObjectMap.ContainsKey (guest)) {
					GameObject guestGO = GuestController.Instance.guestToGameObjectMap [guest];
					guestGO.transform.localPosition = new Vector3(guestGO.transform.localPosition.x,
            		guestGO.transform.localPosition.y + moveBy, guestGO.transform.localPosition.z);
				}


            }
        }

		Display display = liftGo.GetComponent<Display> ();
		display.currentSpeed = moveBy;
		display.distanceToTargetFloor = distanceToTargetFloor;
		display.goingUp = lift.goingUp;
		display.goingDown = lift.goingDown;
        display.CurrentFloor = lift.currentFloor;
        

        
	



	}

	public void SetTargetFloor (Lift lift, int floor, float distanceToTargetFloor) {
		
		GameObject liftGo = GetLiftGameObject (lift);
		
		
		Display display = liftGo.GetComponent<Display> ();
		display.TargetFloor = floor;
		display.distanceToTargetFloor = distanceToTargetFloor;



	}
}

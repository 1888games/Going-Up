using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviourSingleton<LiftController> {

	public List<GameObject> liftGameObjects;

	public Dictionary<GameObject, Lift> gameObjectToLiftMap;
	public Dictionary<Lift, GameObject> liftToGameObjectMap;

	public bool liftsReady = false;

    float doorOpenValue = 0.9f;
    float doorClosedValue = 0.35f;

    public float frontDoorsSpeed = 0.01f;
    public float backDoorsSpeed = 0.0175f;

	// Use this for initialization
	void Start () {

		gameObjectToLiftMap = new Dictionary<GameObject, Lift> ();
		liftToGameObjectMap = new Dictionary<Lift, GameObject> ();

		
	}
	
	// Update is called once per frame
	void Update () {

		if (liftsReady) {

			foreach (Lift lift in liftToGameObjectMap.Keys) {

				lift.ManageLift ();

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

        GameObject liftGo = GetLiftGameObject(lift);
        Transform liftTransform = GetLiftGameObject(lift).transform;

        Doors doors = liftGo.GetComponent<Doors>();

        doors.frontDoorLeft.localPosition = new Vector3(
        Mathf.Clamp(doors.frontDoorLeft.localPosition.x - frontDoorsSpeed, -doorOpenValue, -doorClosedValue),
        doors.frontDoorLeft.localPosition.y, doors.frontDoorLeft.localPosition.z);

        doors.frontDoorRight.localPosition = new Vector3(
        Mathf.Clamp(doors.frontDoorRight.localPosition.x + frontDoorsSpeed, doorClosedValue, doorOpenValue),
        doors.frontDoorRight.localPosition.y, doors.frontDoorRight.localPosition.z);

        doors.backDoorRight.localPosition = new Vector3(
        Mathf.Clamp(doors.backDoorRight.localPosition.x + backDoorsSpeed, doorClosedValue, doorOpenValue),
        doors.backDoorRight.localPosition.y, doors.backDoorRight.localPosition.z);

        doors.backDoorLeft.localPosition = new Vector3(
        Mathf.Clamp(doors.backDoorLeft.localPosition.x - backDoorsSpeed, -doorOpenValue, -doorClosedValue),
        doors.backDoorLeft.localPosition.y, doors.backDoorLeft.localPosition.z);
        
        if (doors.frontDoorRight.localPosition.x >= doorOpenValue) {

            lift.doorsOpening = false;

        }





    }

    public void CloseDoors(Lift lift) {

        GameObject liftGo = GetLiftGameObject(lift);

        Transform liftTransform = GetLiftGameObject(lift).transform;

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


	}

	public void SetLiftPositionFromFloor (Lift lift, int floor) {
	
		Transform liftTransform = GetLiftGameObject (lift).transform;

		liftTransform.localPosition = new Vector3 (liftTransform.localPosition.x,
		(float)floor, liftTransform.localPosition.z);



	}

	public void MoveLift (Lift lift, float moveBy, float distanceToTargetFloor) {
		
		GameObject liftGo = GetLiftGameObject (lift);

		Transform liftTransform = GetLiftGameObject (lift).transform;

		liftTransform.localPosition = new Vector3 (liftTransform.localPosition.x,
			liftTransform.localPosition.y + moveBy, liftTransform.localPosition.z);

		Display display = liftGo.GetComponent<Display> ();
		display.currentSpeed = moveBy;
		display.distanceToTargetFloor = distanceToTargetFloor;
	



	}

	public void SetTargetFloor (Lift lift, int floor, float distanceToTargetFloor) {
		
		GameObject liftGo = GetLiftGameObject (lift);
		
		
		Display display = liftGo.GetComponent<Display> ();
		display.TargetFloor = floor;
		display.distanceToTargetFloor = distanceToTargetFloor;



	}
}

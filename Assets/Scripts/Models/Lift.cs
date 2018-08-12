using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift {


	public int id;
	public int currentFloor = 0;
	public int targetFloor = 0;

	public bool doorsClosed = true;
    public bool doorsOpen = false;
	public float distanceToTargetFloor = 0f;
	public bool accelerating = false;
	public bool decelerating = false;

    public bool doorsClosing = false;
    public bool doorsOpening = false;

	float maximumSpeed = 0.02f;
	float currentSpeed = 0f;
	float accelerateSpeed = 0.002f;
	float decelerateSpeed = 0.004f;
	float deceleratePoint = 0.3f;

	public Hotel hotel;

	public Lift (int id, Hotel hotel) {

		this.id = id;

		this.currentFloor = Random.Range (0, 9);
		this.targetFloor = currentFloor;

	
	}

	public void ManageLift () {

		if (Mathf.Abs(distanceToTargetFloor) > 0f && doorsClosed) {

			MoveLift ();
			return;

		}

        if (doorsClosing) {

            LiftController.Instance.CloseDoors(this);
            return;
        }

        if (doorsOpening) {

            LiftController.Instance.OpenDoors(this);
            return;
        }
		

	}


	public void SetTargetFloor (int floor) {

		this.targetFloor = floor;

		distanceToTargetFloor = 1f * (float)(targetFloor - currentFloor);
		accelerating = true;
		decelerating = false;

		LiftController.Instance.SetTargetFloor (this,floor, distanceToTargetFloor);
		
		


	}

	


	void MoveLift () {

		float direction = 1f;

		if (distanceToTargetFloor > 0f) {
			direction = -1f;
		}

		if (accelerating && currentSpeed < maximumSpeed) {
			currentSpeed += accelerateSpeed;

			if (currentSpeed >= maximumSpeed) {
				currentSpeed = maximumSpeed;
				accelerating = false;
			}
		}

		if (decelerating == false && Mathf.Abs (distanceToTargetFloor) <= deceleratePoint) {
			decelerating = true;
		}

		if (decelerating && currentSpeed >= decelerateSpeed * 2f) {

			currentSpeed -= decelerateSpeed;
			
		}

		distanceToTargetFloor += currentSpeed * direction;

		LiftController.Instance.MoveLift(this, currentSpeed * -direction, distanceToTargetFloor);
		

		if ((distanceToTargetFloor <= 0f && direction == -1f) || (distanceToTargetFloor >= 0f && direction == 1f)) {
			distanceToTargetFloor = 0f;
			decelerating = false;

            LiftController.Instance.ArrivedOnFloor(this);
			
		}

		
			
	}
	
	
}

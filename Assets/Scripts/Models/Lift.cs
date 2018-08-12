using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift {


	public int id;
	public int currentFloor = 0;
	public int targetFloor = 0;

	public bool goingUp = false;
	public bool goingDown = false;

	public bool doorsClosed = true;
    public bool doorsOpen = false;
	public float distanceToTargetFloor = 0f;
	public bool accelerating = false;
	public bool decelerating = false;

    public bool doorsClosing = false;
    public bool doorsOpening = false;

	float maximumSpeed = 0.04f;
	float currentSpeed = 0f;
	float accelerateSpeed = 0.001f;
	float decelerateSpeed = 0.004f;
	float deceleratePoint = 0.3f;

	public float liftOpenTimer = 0f;
	float liftOpenTimeout = 6f;

    bool displaySetup = false;

    public Dictionary<int, Guest> slots;


	public Dictionary<int, bool> liftButtonsPressed;

	public Hotel hotel;

	public Lift (int id, Hotel hotel) {

		this.id = id;

        this.currentFloor = 0;
		this.targetFloor = currentFloor;

		liftButtonsPressed = new Dictionary<int, bool> ();
        slots = new Dictionary<int, Guest>();

		this.hotel = hotel;

        for (int i = 0; i < 6; i++) {

            slots.Add(i, null);
        }
		

	
	}

	public void ManageLift () {

		if (GuestController.Instance.gameActive) {

			if (displaySetup == false) {

				LiftController.Instance.UpdateFloorNumber (this);
				LiftController.Instance.UpdateLiftArrows (this);
			}

			if (doorsClosed) {

				if (Mathf.Abs (distanceToTargetFloor) > 0f) {
					if (currentFloor != targetFloor) {
						CalculateTargetFloor ();
					}
					MoveLift ();

					return;
				}

				liftOpenTimer += Time.deltaTime;

				CalculateTargetFloor ();


			}



			if (doorsOpen) {

				liftOpenTimer += Time.deltaTime;

				if (liftOpenTimer >= liftOpenTimeout) {

					CalculateTargetFloor ();

					if (targetFloor != currentFloor) {

						doorsClosing = true;
						doorsOpen = false;
						doorsClosed = false;
						doorsOpening = false;
						liftOpenTimeout = 0f;

					}
				}

			}


			if (doorsClosing) {

				//Debug.Log (id + " closing...");
				LiftController.Instance.CloseDoors (this);
				return;
			}

			if (doorsOpening) {

				doorsClosed = false;

				LiftController.Instance.OpenDoors (this);
				return;
			}


		}
		

	}


	void CalculateTargetFloor () {

        //Debug.Log("CALCULATE TARGET...");

        int newFloor = currentFloor;

        if (goingUp || goingDown == false) {

            for (int i = currentFloor + 1; i < hotel.floors.Count; i++) {

                if (liftButtonsPressed[i]) {
               //     Debug.Log("Found new floor: " + i);
                    newFloor = i;
                    break;
                }
            }

            if (newFloor == currentFloor && currentFloor > 0) {
                goingUp = false;
                LiftController.Instance.UpdateLiftArrows(this);
            }            

        }

        if (goingDown || goingUp == false) {

            for (int i = currentFloor - 1; i >= 0; i--) {

                if (liftButtonsPressed[i]) {

                    if (currentFloor == newFloor) {
                        newFloor = i;
                    }
                    else {

                        if (Mathf.Abs(currentFloor - i) < Mathf.Abs(currentFloor - newFloor)) {

                            newFloor = i;
                        }
                    }
                    break;
                }

            }

            if (newFloor == currentFloor && currentFloor < 9) {
                goingDown = false;
                LiftController.Instance.UpdateLiftArrows(this);
            }


        }
        if (newFloor != targetFloor) {
            SetTargetFloor(newFloor);
        }

    }


	public void SetTargetFloor (int floor) {

        Debug.Log("Set new target floor: " + floor);

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
			accelerating = false;
			doorsClosed = false;
			doorsOpening = true;
			liftOpenTimer = 0f;

            LiftController.Instance.ArrivedOnFloor(this);
			
		}

		
			
	}
	
	
}

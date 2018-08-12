using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest {

    public Lift lift;   
    public int currentFloor;
    public int targetFloor;

    public float patience = 1f;
    public float timeWaiting = 0f;
    public float maxWaitingTime = 0f;
    public float timeInLift = 0f;

    public bool guestReadyForLift = false;
    public bool reachedDestination = false;
	public bool lostPatience = false;
	public bool complained = false;
	public float complainPoint = 0f;
  

    public FaceCanvas hud;

    public Guest(int currentFloor, int targetFloor, float waitingTime) {

        this.currentFloor = currentFloor;
        this.targetFloor = targetFloor;

        this.maxWaitingTime = waitingTime;

		this.complainPoint = Random.Range (0.4f, 0.8f);

    }


    public void UpdateGuest() {

        if (reachedDestination == false && GuestController.Instance.gameActive) {

            if (lift != null) {

                timeInLift += Time.deltaTime;

            }

            else {

                if (guestReadyForLift) {
                    timeWaiting += Time.deltaTime;
                }

               
            }

            patience = Mathf.Max(0f, 1f - (timeInLift + timeWaiting) / maxWaitingTime);

			if (patience <= 0f && lostPatience == false) {

				lostPatience = true;
			
				GuestController.Instance.TakeTheStairs (this);

			}

			if (patience < complainPoint && complained == false) {
		
				complained = true;
				SoundController.Instance.Complain ();

			}

           
            GuestController.Instance.UpdatePatienceBar(this);

        }

        GuestController.Instance.CheckOnVisibleFloor(this);

    }
}

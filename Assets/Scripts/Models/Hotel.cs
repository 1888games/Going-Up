using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotel {


	public List<Guest> guests;
	public List<Floor> floors;
	public List<Lift> lifts;

	public Hotel (int numberOfFloors) {

		guests = new List<Guest> ();
		
		lifts = LiftController.Instance.CreateLifts (this);
        floors = HotelController.Instance.CreateFloors(numberOfFloors);

		for (int i = 0; i < lifts.Count; i++) {


			for (int y = 0; y < floors.Count; y++) {

				lifts [i].liftButtonsPressed.Add (y, false);
			}

		}
		
	}
}

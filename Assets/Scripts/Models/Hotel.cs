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
		
	}
}

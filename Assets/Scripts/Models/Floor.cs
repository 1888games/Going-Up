using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor {

	public List<Guest> guestsOnFloor;
	public int id;
    public string shortName;
    public string description;
    public float floorHeight;
    public string rooms;


	public Floor (FloorData data) {

        this.id = data.id;
        this.shortName = data.floorShortName;
        this.description = data.floorDescription;
        this.floorHeight = data.floorHeight;
        this.rooms = data.rooms;

        HotelController.Instance.CreateFloor(this);


	}
}

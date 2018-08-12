using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData {


    public int id;
    public float floorHeight;
    public string floorShortName;
    public string floorDescription;
    public string rooms;
    
	public FloorData(int id, float height, string shortName, string longName, string rooms) {

        this.id = id;
        this.floorHeight = height;
        this.floorShortName = shortName;
        this.floorDescription = longName;
        this.rooms = rooms;
    }
}

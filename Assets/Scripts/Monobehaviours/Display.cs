using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {


	public int TargetFloor = 0;
	public int CurrentFloor = 0;

	public bool goingUp = false;
	public bool goingDown = false;

	public float currentSpeed = 0f;
	public float distanceToTargetFloor = 0f;

	public bool doorsOpening = false;
	public bool doorsClosing = false;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (CurrentFloor > TargetFloor) {
			goingDown = true;
		} else {
			goingDown = false;
		}

		if (CurrentFloor < TargetFloor) {
			goingUp = true;

		} else {
			goingUp = false;
		}
		
	}
}

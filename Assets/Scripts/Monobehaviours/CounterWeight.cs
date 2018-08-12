using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class CounterWeight : MonoBehaviour {

	public Transform weight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		weight.position = new Vector3 (transform.position.x - 2f, 9f - transform.position.y, 3.25f);
		
		
	}
}

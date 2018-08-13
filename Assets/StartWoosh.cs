using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartWoosh : MonoBehaviour {

	public float target = 31f;
	public float perFrame = 0.5f;

	// Use this for initialization
	void Start () {
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.eulerAngles.y > target) {
			transform.eulerAngles = new Vector3 (25f, transform.eulerAngles.y - perFrame, 0f);
			perFrame = Mathf.Max(0.1f, perFrame - 0.02f);
		}

		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowLift : MonoBehaviour {

    public List<Transform> lifts;
    public int liftSelected = 0;
    public bool lockOnLift = false;

    public float bulgeSpeed = 0.015f;
    public float bulgeBy = 0.15f;
    public float cylinderScale = 1.25f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (lockOnLift) {

            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
               3.3f + lifts[liftSelected].transform.position.y,
                Camera.main.transform.position.z);

     
        }


        transform.localScale = new Vector3(transform.localScale.x + bulgeSpeed, transform.localScale.y, transform.localScale.z + bulgeSpeed);
        
        if (transform.localScale.x >= cylinderScale + bulgeBy) {

            transform.localScale = new Vector3(cylinderScale, transform.localScale.y, cylinderScale);
        }

		//	transform.GetComponent<Renderer>().material
        
		
	}


    public void MoveCylinder(int change) {

        liftSelected += change;

        transform.localPosition = new Vector3(-2f + liftSelected * 2f, transform.localPosition.y, transform.localPosition.z);
    }

    public void SetCylinder(int newSelection) {

        liftSelected = newSelection;
        transform.localPosition = new Vector3(-2f + liftSelected * 2f, transform.localPosition.y, transform.localPosition.z);

    }
    
}

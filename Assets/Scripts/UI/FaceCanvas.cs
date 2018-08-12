using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FaceCanvas : MonoBehaviour {

    public Canvas hud;

    public RectTransform patienceBar;
    public TextMeshProUGUI targetFloorText;
    public Image patienceBarImage;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        float yCalc = 315f + (90f - transform.eulerAngles.y);

        //Debug.Log(transform.eulerAngles.y + " " + yCalc);

        hud.transform.localEulerAngles = new Vector3(0f, yCalc, 0f);

        
    }
}
    
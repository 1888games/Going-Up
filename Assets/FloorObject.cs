using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloorObject : MonoBehaviour {

    public List<FloorDisplay> floorDisplays;
    public List<TextMeshProUGUI> floorNumbers;
    public TextMeshProUGUI roomNumbers;
    public TextMeshProUGUI floorName;


    public Renderer tiledFloor;
    public Renderer carpet;

    public Material tiledSolid;
    public Material tiledTransparent;
    public Material carpetSolid;
    public Material carpetTransparent;

    public GameObject carpetGO;
    
	// Use this for initialization
	void Start () {
	    
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotelController : MonoBehaviourSingleton<HotelController> {

    public Hotel hotel;

    public List<FloorData> floorData;
    public GameObject floorPrefab;
    public List<GameObject> floorGameObjects;

    public Dictionary<Floor, GameObject> floorToGameObjectMap;
    public Dictionary<GameObject, Floor> gameObjectToFloorMap;
    public CameraFollowLift selectionCylinder;

    public int floorBeingViewed = 0;
    public int liftBeingControlled = 0;

    public GameObject worldGameObject;

    void SetupFloorData() {

        floorData = new List<FloorData>();

        floorData.Add(new FloorData(0, 0f, "G", "reception", "toilets"));
        floorData.Add(new FloorData(1, 1f, "1", "restaurant", "rooms 101-112"));
        floorData.Add(new FloorData(2, 2f, "2", "indoor pool", "rooms 201-233"));
        floorData.Add(new FloorData(3, 3f, "3", "fitness centre", "rooms 301-320"));
        floorData.Add(new FloorData(4, 4f, "4", "laundry", "rooms 401-465"));
        floorData.Add(new FloorData(5, 5f, "5", "games room", "rooms 501-538"));
        floorData.Add(new FloorData(6, 6f, "6", "conference room", "rooms 601-642"));
        floorData.Add(new FloorData(7, 7f, "7", "business centre", "rooms 701-781"));
        floorData.Add(new FloorData(8, 8f, "8", "bar", "rooms 801-843"));
        floorData.Add(new FloorData(9, 9f, "9", "roof terrace", "pool"));




    }



    // Use this for initialization
    void Start() {

        Destroy(GameObject.FindGameObjectWithTag("Template"));
        floorToGameObjectMap = new Dictionary<Floor, GameObject>();
        gameObjectToFloorMap = new Dictionary<GameObject, Floor>();
        SetupFloorData();

        this.hotel = new Hotel(10);

        LiftController.Instance.SetLiftPositionFromFloor(hotel.lifts[0], hotel.lifts[0].currentFloor);
        LiftController.Instance.SetLiftPositionFromFloor(hotel.lifts[1], hotel.lifts[1].currentFloor);
        LiftController.Instance.SetLiftPositionFromFloor(hotel.lifts[2], hotel.lifts[2].currentFloor);


        LiftController.Instance.TestLift(0, Random.Range(0, 9));
        LiftController.Instance.TestLift(1, Random.Range(0, 9));
        LiftController.Instance.TestLift(2, Random.Range(0, 9));

        ShowCorrectFloor();


    }

    // Update is called once per frame
    void Update() {

        int currentFloor = CheckUpDown();
        CheckLeftRight();

        //   int currentFloor = Mathf.RoundToInt((Camera.main.transform.position.y - 3.3f));

        if (currentFloor != floorBeingViewed) {


            floorBeingViewed = currentFloor;
            ShowCorrectFloor();
        }


    }

   void CheckLeftRight() {

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && liftBeingControlled > 0) {

            liftBeingControlled -= 1;
            selectionCylinder.MoveCylinder(-1);

        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.W)) && liftBeingControlled < 2) {

            liftBeingControlled += 1;
            selectionCylinder.MoveCylinder(+1);
        }

    }

    int CheckUpDown() {

        if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && floorBeingViewed > 0) {

            return floorBeingViewed - 1;
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && floorBeingViewed < hotel.floors.Count - 1) {

            return floorBeingViewed + 1;
        }

        return floorBeingViewed;


    }

    void ShowCorrectFloor() {

        foreach(GameObject floorGO in floorGameObjects) {

            Floor floor = gameObjectToFloorMap[floorGO];

            FloorObject floorObj = floorGO.GetComponent<FloorObject>();

            if (floor.id == floorBeingViewed) {

                floorGO.SetActive(true);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, floor.floorHeight + 3.3f, Camera.main.transform.position.z);

            }

            else {


                floorGO.SetActive(false);

            }

            
        }
        
    }

    public void CreateFloor(Floor floor) {

        GameObject floorGO = Instantiate(floorPrefab, worldGameObject.transform);

        floorGO.transform.localPosition = new Vector3(0f, floor.floorHeight, 0f);
        floorGO.transform.localScale = Vector3.one;

        floorToGameObjectMap.Add(floor, floorGO);
        gameObjectToFloorMap.Add(floorGO, floor);
        floorGameObjects.Add(floorGO);

        FloorObject floorObj = floorGO.GetComponent<FloorObject>();

        floorObj.roomNumbers.text = floor.rooms;
        floorObj.floorName.text = floor.description;
        floorObj.floorNumbers[0].text = floor.shortName;
        floorObj.floorNumbers[1].text = floor.shortName;




    }
    
    public List<Floor> CreateFloors(int numberOfFloors) {

        List<Floor> floors = new List<Floor>();

        for(int i = 0; i < numberOfFloors; i++) {

            //Floor floor = new Floor(floorData[i]);
            floors.Add(new Floor(floorData[i]));
            

        }

        return floors;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getCoordinates : MonoBehaviour {
    public GameObject coordinateText;
    Vector3 startingCoordinates;

	// Use this for initialization
	void Start () {
		startingCoordinates = Camera.main.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mainCameraPos = Camera.main.transform.position;
        string mainCameraPosString = "(" + mainCameraPos.x.ToString() + ", " + mainCameraPos.y.ToString() + ", " + mainCameraPos.z + ")";
        string startingCoordString = "(" + startingCoordinates.x.ToString() + ", " + startingCoordinates.y.ToString() + ", " + startingCoordinates.z + ")";
        coordinateText.GetComponent<Text>().text = "Starting Position: " + startingCoordString + "\nMain Camera: " + mainCameraPosString;
	}
}

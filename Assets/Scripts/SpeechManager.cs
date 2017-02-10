using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

public class SpeechManager : MonoBehaviour
{
    public AudioSource ass;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public GameObject coordinateText;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;

    Vector3 startingCoordinates;
    bool displayCoords = true;

    Vector3 p1Coords;
    Vector3 p2Coords;
    Vector3 p3Coords;

    Vector4 rightWall;
    Vector4 forwardWall;
    Vector4 floorWall;
    Vector3 weija;

    Vector3 rightCoords;
    Vector3 forwardCoords;
    Vector3 floorCoords;

    bool setWalls = false;

    bool setRightWall = false;
    bool setForwardWall = false;
    bool setFloorWall = false;


    public GameObject wall1;
    calc3DEquations calc3d;

    // Use this for initialization
    void Start()
    {
         calc3d = GetComponent<calc3DEquations>();

        startingCoordinates = Camera.main.transform.position;

        keywords.Add("Dad", () =>
        {
            setRightWall = true;
        });

        keywords.Add("Bottle", () =>
        {
            setForwardWall = true;
        });

        keywords.Add("Floor", () =>
        {
            setFloorWall = true;
        });

        keywords.Add("Show World", () =>
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        });

        keywords.Add("Hide World", () =>
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        });

        keywords.Add("Show Coordinates", () =>
        {
            displayCoords = true;
        });

        keywords.Add("Hide Coordinates", () =>
        {
            displayCoords = false;
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void Update()
    {
        p1Coords = p1.transform.position;
        p2Coords = p2.transform.position;
        p3Coords = p3.transform.position;

        if (displayCoords)
        {
            Vector3 mainCameraPos = Camera.main.transform.position;
            string mainCameraPosString = getThreeTuple(mainCameraPos.x, mainCameraPos.y, mainCameraPos.z);
            string startingCoordString = getThreeTuple(startingCoordinates.x, startingCoordinates.y, startingCoordinates.z);

            //string p1CoordsString = getThreeTuple(p1Coords.x, p1Coords.y, p1Coords.z);
            //string p2CoordsString = getThreeTuple(p2Coords.x, p2Coords.y, p2Coords.z);
            //string p3CoordsString = getThreeTuple(p3Coords.x, p3Coords.y, p3Coords.z);

            string p1CoordsString = getThreeTuple(rightCoords.x, rightCoords.y, rightCoords.z);
            string p2CoordsString = getThreeTuple(forwardCoords.x, forwardCoords.y, forwardCoords.z);
            string p3CoordsString = getThreeTuple(floorCoords.x, floorCoords.y, floorCoords.z);

            string weijaCoordsString = getThreeTuple(weija.x, weija.y, weija.z);

            coordinateText.GetComponent<Text>().text = "Starting Position: " + startingCoordString + "\nMain Camera: " + mainCameraPosString + "\np1 Position: " + p1CoordsString + "\np2 Position: " + p2CoordsString + "\np3 Position: " + p3CoordsString + "\n weija: " + weijaCoordsString;
        } else
        {
            coordinateText.GetComponent<Text>().text = "";
        }

        if (setRightWall)
        {
            setRightWall = false;
            rightWall = calc3d.getPlane(p1Coords, p2Coords, p3Coords);
            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ball.transform.position = p1Coords;
            rightCoords = p1Coords;

            ass.Play();
    
        } else if (setForwardWall)
        {
            setForwardWall = false;
            forwardWall = calc3d.getPlane(p1Coords, p2Coords, p3Coords);

            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ball.transform.position = p1Coords;

            forwardCoords = p1Coords;

            ass.Play();

        } else if (setFloorWall)
        {
            setFloorWall = false;

            floorWall = calc3d.getPlane(p1Coords, p2Coords, p3Coords);
            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ball.transform.position = p1Coords;
            floorCoords = p1Coords;

            Vector3[] intersection1 = calc3d.getIntersectionFromPlanes(rightWall, floorWall);
            Vector3[] intersection2 = calc3d.getIntersectionFromPlanes(forwardWall, floorWall);

            weija = calc3d.getPointOfIntersection(intersection1, intersection2);

            ass.Play();
            setWalls = false;
            GameObject ball1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball1.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ball1.transform.position = weija;

        }
    }

    private string getThreeTuple(float a, float b, float c)
    {
        return "(" + string.Format("{0:N8}", a) + ", " + string.Format("{0:N8}", b) + ", " + string.Format("{0:N8}", c) + ")";
    }

    void CreatePlane(GameObject reference)
    {
        wall1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall1.transform.rotation = reference.transform.rotation;
        wall1.transform.position = reference.transform.position;
    }
}
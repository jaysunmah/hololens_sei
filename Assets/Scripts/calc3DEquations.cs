using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calc3DEquations : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    //returns <a,b,c,d> vector such that ax + by + cz + d = 1
    public Vector4 getPlane(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 p1p2 = p2 - p1;
        Vector3 p1p3 = p3 - p1;
        Vector3 normal = Vector3.Cross(p1p2, p1p3);

        float d = -1 * normal.x * p1.x - normal.y * p1.y - normal.z * p1.z;

        Vector4 plane = new Vector4(normal.x, normal.y, normal.z, d);

        return plane;
    }

    public Vector3[] getIntersectionFromPlanes(Vector4 plane1, Vector4 plane2)
    {
        float a = plane1.x;
        float b = plane1.y;
        float c = plane1.z;
        float d = plane1.w;

        float e = plane2.x;
        float f = plane2.y;
        float g = plane2.z;
        float h = plane2.w;

        float k = (-1 * b * a * g + b * e * c - c * e * b + c * a * f) / (a * a * g - a * e * c);
        float l = -1 * (c / a) * ((a * (1 - h) + e * (d - 1)) / (a * g - e * c)) + (1 / a) - (d / a);

        float m = (e * b - a * f) / (a * g - e * c);
        float n = (a * (1 - h) + e * (d - 1)) / (a * g - e * c);

        Vector3 origin = new Vector3(l, 0, n);
        Vector3 slope = new Vector3(k, 1, m);

        Vector3[] result = new Vector3[2] { origin, slope };
        return result;
    }

    public Vector3 getPointOfIntersection(Vector3[] l1, Vector3[] l2)
    {
        float x1 = l1[0].x;
        float y1 = l1[0].y;
        float z1 = l1[0].z;

        float a1 = l1[1].x;
        float b1 = l1[1].y;
        float c1 = l1[1].z;

        float x2 = l2[0].x;
        float y2 = l2[0].y;
        float z2 = l2[0].z;

        float a2 = l2[1].x;
        float b2 = l2[1].y;
        float c2 = l2[1].z;

        float isIntersecting = ((-1 * (c1 * a2) + c2 * a1) / (b1 * a2 - b2 * a1)) * ((b1 / a1) * (x1 - x2) + (y2 - y1)) + (c1 / a1) * (x1 - x2) + (z2 - z1);

        Debug.Log(isIntersecting);

        //if (Mathf.Abs(isIntersecting) > 0.001)
        //{
         //   return new Vector3();
        //}

        float t = ((b1 / a1) * (x1 - x2) + (y2 - y1)) / ((b1 * a2 - b2 * a1) / (a1));
        float s = (((b1 * a2 - b2 * a1) / (a1 * a2)) * ((b1 / a1) * (x1 - x2) + (y2 - y1)) + (x2 - x1)) / (a1);

        //Debug.Log(t);
        //Debug.Log(s);

        float x = x1 + a1 * t;
        float y = y1 + b1 * t;
        float z = z1 + c1 * t;

        return new Vector3(x, y, z);
    }

}

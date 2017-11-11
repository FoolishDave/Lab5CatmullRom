using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class CatmullRomCurveInterpolation : MonoBehaviour {
	
	const int NumberOfPoints = 8;
	Vector3[] controlPoints;
	
	const int MinX = -5;
	const int MinY = -5;
	const int MinZ = 0;

	const int MaxX = 5;
	const int MaxY = 5;
	const int MaxZ = 5;
	
	double time = 0;
    int segment = 0;
	const double DT = 0.01;
	
	/* Returns a point on a cubic Catmull-Rom/Blended Parabolas curve
	 * u is a scalar value from 0 to 1
	 * segment_number indicates which 4 points to use for interpolation
	 */
	Vector3 ComputePointOnCatmullRomCurve(double u, int segmentNumber)
	{
		Vector3 point = new Vector3();

	    Vector3 c3, c2, c1, c0;
	    float t = 0.5f;
	    Vector3 pmin1 = segmentNumber == 0 ? controlPoints[NumberOfPoints - 1] : controlPoints[segmentNumber - 1];
	    Vector3 pmin2 = segmentNumber < 2 ? controlPoints[NumberOfPoints - 2] : controlPoints[segmentNumber - 2];
	    Vector3 p1 = segmentNumber == NumberOfPoints - 1 ? controlPoints[0] : controlPoints[segmentNumber + 1];
	    Vector3 p0 = controlPoints[segmentNumber];

	    c0 = pmin1;
	    c1 = -t * pmin2 + t * p0;
	    c2 = 2 * t * pmin2 + (t - 3) * pmin1 + (3 - 2*t) * p0 - t * p1;
	    c3 = -t * pmin2 + (2 - t) * pmin1 + (t - 2) * p0 + t * p1;

	    point = c3 * (float)(u * u * u) + c2 * (float)(u * u) + c1 * (float)u + c0;
		
		return point;
	}
	
	void GenerateControlPointGeometry()
	{
		for(int i = 0; i < NumberOfPoints; i++)
		{
			GameObject tempcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			tempcube.transform.localScale -= new Vector3(0.8f,0.8f,0.8f);
			tempcube.transform.position = controlPoints[i];
		}	
	}
	
	// Use this for initialization
	void Start () {

		controlPoints = new Vector3[NumberOfPoints];
		
		// set points randomly...
		controlPoints[0] = new Vector3(0,0,0);
		for(int i = 1; i < NumberOfPoints; i++)
		{
			controlPoints[i] = new Vector3(Random.Range(MinX,MaxX),Random.Range(MinY,MaxY),Random.Range(MinZ,MaxZ));
		}
		/*...or hard code them for testing
		controlPoints[0] = new Vector3(0,0,0);
		controlPoints[1] = new Vector3(0,0,0);
		controlPoints[2] = new Vector3(0,0,0);
		controlPoints[3] = new Vector3(0,0,0);
		controlPoints[4] = new Vector3(0,0,0);
		controlPoints[5] = new Vector3(0,0,0);
		controlPoints[6] = new Vector3(0,0,0);
		controlPoints[7] = new Vector3(0,0,0);
		*/
		
		GenerateControlPointGeometry();
	}
	
	// Update is called once per frame
	void Update () {
		
		time += DT;
			
		// TODO - use time to determine values for u and segment_number in this function call
		
		Vector3 temp = ComputePointOnCatmullRomCurve(time,segment);
		transform.position = temp;

	    if (time >= 1)
	    {
	        time = 0;
	        segment++;
	        if (segment == NumberOfPoints) segment = 0;
	    }
	}
}

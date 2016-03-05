using UnityEngine;
using System.Collections;

public class LineDebug : MonoBehaviour {

	public GameObject point1;
	public GameObject point2;
	
	public GameObject point3;
	public GameObject point4;
	
	void Update () {
		Debug.DrawLine (point1.transform.position, point2.transform.position,Color.red,Time.deltaTime);
		Debug.DrawLine (point3.transform.position, point4.transform.position,Color.blue,Time.deltaTime);
		
		bool intersect;
		Vector3 vec = HelperUtil.segmentLineIntersection (point1.transform.position.ToVector2(), point2.transform.position.ToVector2(), point3.transform.position.ToVector2(), point4.transform.position.ToVector2(), out intersect);
		Debug.LogError(vec.ToVector2() + " " + intersect);
	}
}

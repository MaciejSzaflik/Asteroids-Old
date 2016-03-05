using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	public float rotationSpeed = 5.0f;

	void Update () {
		transform.RotateAround(transform.parent.position,new Vector3(0,0,1),rotationSpeed);
	}
}

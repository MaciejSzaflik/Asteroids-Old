using UnityEngine;
using System.Collections;

public class ParalaxBackground : MonoBehaviour {
	GameObject connectedGameObject = null;
	Material backgroundMaterial;

	Vector3 lastPosition;
	Vector3 lastDelta;

	public float paralaxAmount = 0;
	public float sizeOfBackground = 1;

	void Start () {
		Bounds cameraBounds = Camera.main.OrthographicBounds();
		Bounds objectBounds = GetComponent<Renderer>().bounds;

		float scaleParam = cameraBounds.size.x/objectBounds.size.x;
		transform.localScale*=scaleParam*sizeOfBackground;

		backgroundMaterial = this.GetComponent<Renderer>().material;
	}

	public void SetConnectedGameObject(GameObject toConnect)
	{
		connectedGameObject = toConnect;
		lastPosition = toConnect.transform.position;
	}

	public void Update()
	{
		if(connectedGameObject == null)
			return;
		lastDelta = lastPosition - connectedGameObject.transform.position;

		if(lastDelta.magnitude>5)
			lastDelta = lastDelta.normalized*0.01f;
		lastPosition = connectedGameObject.transform.position;
		backgroundMaterial.mainTextureOffset += lastDelta.ToVector2()*paralaxAmount;

	}
	

}

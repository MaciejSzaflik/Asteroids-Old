using UnityEngine;
using System.Collections.Generic;

public class PositionManager : MonoBehaviour {

	public List<GameObject> objectToCheck = new List<GameObject>();

	private static PositionManager instance = null;
	
	public static PositionManager Instance
	{
		get
		{
			if(instance==null)
				instance = GameObject.FindObjectOfType<PositionManager>();
			return instance;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 minBoundsCamera = Camera.main.BoundsMin()*1.05f;
		Vector2 maxBoundsCamera = Camera.main.BoundsMax()*1.05f;
		Bounds cameraBounds = Camera.main.OrthographicBounds();
		for(int i =0;i<objectToCheck.Count;i++)
		{	
			Renderer rend = objectToCheck[i].GetComponent<Renderer>();

			if(!rend.isVisible)
			{
				Vector3 objectBoundsExtends = rend.bounds.extents;
				Vector2 offsetValue = new Vector2(0.3f,0.3f);
				Vector2 minBounds = (objectToCheck[i].transform.position - objectBoundsExtends).ToVector2();
				Vector2 maxBounds = (objectToCheck[i].transform.position + objectBoundsExtends).ToVector2();
				if(minBounds.x  < minBoundsCamera.x )
					objectToCheck[i].transform.position = objectToCheck[i].transform.position.AddToX(  cameraBounds.size.x  *1.05f);
				else if(maxBounds.x + offsetValue.x > maxBoundsCamera.x )
					objectToCheck[i].transform.position = objectToCheck[i].transform.position.AddToX(  -cameraBounds.size.x *1.05f);

				if(minBounds.y < minBoundsCamera.y )
					objectToCheck[i].transform.position = objectToCheck[i].transform.position.AddToY(  cameraBounds.size.y *1.05f);
				else if(maxBounds.y > maxBoundsCamera.y )
					objectToCheck[i].transform.position = objectToCheck[i].transform.position.AddToY(  -cameraBounds.size.y *1.05f);
			}
		}
	}
}



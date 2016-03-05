using UnityEngine;
using System.Collections;
using System;

public class LinearColorTransform : ITransform
{
	private Color startColor;
	private Color targetColor;
	private float lenght;
	private float timePassed;
	private GameObject connectedGameObject;
	

	public Action<GameObject,Color> setter;

	public LinearColorTransform (Color startCl,Color targetCl, float lenghtOfTransform,Action<GameObject,Color> setColor )
	{
		targetColor = targetCl;
		startColor = startCl;
		lenght = lenghtOfTransform;
		setter = setColor;
		timePassed = 0;
	}
	public void SetGameObject(GameObject a)
	{
		connectedGameObject = a;
	}
	public bool Do(float deltaTime)
	{
		if(connectedGameObject == null)
			return false;
		
		timePassed+=deltaTime;
		float state = timePassed/lenght;
		if(state >= 1.0f)
		{
			setter(connectedGameObject,targetColor);
			return false;
		}
		else
		{
			setter(connectedGameObject, Color.Lerp(startColor,targetColor,state));
		}
		return true;
	}
	
}


using UnityEngine;
using System.Collections;
using System;

public class PingPongColorTransform : ITransform
{
	private Color endColor;
	private Color highLightColor;
	private float lenght;
	private float period ;
	private float timePassed;
	private bool ethernalTransformation;
	private GameObject connectedGameObject;
	public Action<GameObject,Color> setter;
	public PingPongColorTransform (Color endCol,Color highLight, float lenghtOfTransform, float periods,Action<GameObject,Color> colorSetter ,bool neverEnding = false)
	{
		period = periods;
		highLightColor = highLight;
		endColor = endCol;
		lenght = lenghtOfTransform;
		period = periods;
		ethernalTransformation = neverEnding;
		setter = colorSetter;
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
		if(state >= 1.0f && ethernalTransformation)
		{
			state = 0.0f;
			timePassed = 0;
		}
		else if(state >= 1.0f)
		{
			setter(connectedGameObject,endColor);
			return false;
		}

		setter(connectedGameObject, Color.Lerp(endColor,highLightColor,Mathf.Sin(state*Mathf.PI*2*period)));

		return true;
	}

}


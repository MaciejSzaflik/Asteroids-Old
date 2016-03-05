using UnityEngine;
using System.Collections;

public class LinearScaleTransform : ITransform
{
	private Vector3 startScale;
	private Vector3 targetScale;
	private float lenght;
	private float timePassed;
	private Transform connectedTransform;
	public LinearScaleTransform (Vector3 startSc,Vector3 targetSc, float lenghtOfTransform )
	{
		startScale = startSc;
		targetScale = targetSc;
		lenght = lenghtOfTransform;
		timePassed = 0;
	}
	public void SetGameObject(GameObject a)
	{
		connectedTransform = a.transform;
	}
	public bool Do(float deltaTime)
	{
		if(connectedTransform == null)
			return false;
		
		timePassed+=deltaTime;
		float state = timePassed/lenght;
		if(state >= 1.0f)
		{
			connectedTransform.localScale = targetScale;
			return false;
		}
		else
		{
			connectedTransform.localScale = Vector3.Lerp(startScale,targetScale,state);
		}
		return true;
	}
	
}


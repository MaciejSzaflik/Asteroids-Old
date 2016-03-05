using UnityEngine;
using System.Collections.Generic;

public class TransformManager : MonoBehaviour
{
	private static TransformManager instance = null;
	private static int sequenceNumber = 0;
	public static TransformManager Instance
	{
		get
		{
			if(instance==null)
				instance = GameObject.FindObjectOfType<TransformManager>();
			return instance;
		}
	}
	private Dictionary<int,ITransform> activeTransformsList = new Dictionary<int, ITransform>();

	void Update () 
	{
		List<int> toRemove = new List<int>();
		foreach(KeyValuePair<int,ITransform> pair in activeTransformsList)
		{
			if(!pair.Value.Do(Time.deltaTime))
			{
				toRemove.Add(pair.Key);
			}

		}
		for(int i = 0;i<toRemove.Count;i++)
		{
			activeTransformsList.Remove(toRemove[i]);
		}
	}
	public void CancelTransform(int uniqueTransformId)
	{
		if(activeTransformsList.ContainsKey(uniqueTransformId))
		   activeTransformsList.Remove(uniqueTransformId);
	}
	public int AddTransform(GameObject toAdd,ITransform trans)
	{
		sequenceNumber++;
		trans.SetGameObject(toAdd);
		activeTransformsList.Add(sequenceNumber,trans);
		return sequenceNumber;
	}



}



using UnityEngine;
using System.Collections;

public class DestroyMe : MonoBehaviour {
	
	public void ScheduleDestruction(float timeToDestruction)
	{
		Invoke("DestroyObj",timeToDestruction);
	}
	
	private void DestroyObj()
	{
		if(gameObject!=null)
		{
			PositionManager.Instance.objectToCheck.Remove(gameObject);
			Destroy(gameObject);
		}
	}
}

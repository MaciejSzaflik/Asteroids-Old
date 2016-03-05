using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Ship ship = other.gameObject.GetComponent<Ship>();
		if(ship!=null)
		{
			ship.onPowerUp(this.name);
			PositionManager.Instance.objectToCheck.Remove(gameObject);
			Destroy(gameObject);
		}
	}
}

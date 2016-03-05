using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Vector3 startPosition;
	// Use this for initialization
	void Start () {
		Invoke("DestroyBullet",3.0f);
	}

	private void DestroyBullet()
	{
		Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {

		Asteroid ast = other.gameObject.GetComponent<Asteroid>();
		if(ast != null)
		{
			ast.Hit(new MathLine2D(startPosition,this.transform.position));
			DestroyBullet();
		}

		
	}

	public void SetTransform(Vector3 rotation, Vector3 positon)
	{
		this.transform.eulerAngles = rotation;
		this.transform.position = startPosition = positon;
	}

}

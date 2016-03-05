using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public float speed = 4f;
	public float turnspeed = 0.18f;
	public float miliSecondsFire = 0.4f;
	private float lastTimeShot = 0;
	private float immortalityTime = 3;
	private bool immortal = false;
	private int hitPoints;
	private float speedUp = 1.0f;
	public bool Immortal
	{
		get{
			return immortal;
		}
		set
		{
			immortal = value;
		}
	}
	public Rigidbody2D shipBody;
	void Start () {
		shipBody = GetComponent<Rigidbody2D>();
		hitPoints = 3;
		EnableImmortality();
		UI.Instance.SetBar(hitPoints);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.W))
		{
			shipBody.AddRelativeForce (new Vector2(0,speed*speedUp));
		}
		if(Input.GetKey(KeyCode.S))
		{
			shipBody.AddRelativeForce (new Vector2(0,-speed*speedUp));
		}
		if(Input.GetKey(KeyCode.A))
		{
			shipBody.AddTorque(turnspeed*speedUp);
		}
		if(Input.GetKey(KeyCode.D))
		{
			shipBody.AddTorque(-turnspeed*speedUp);
		}
		if(Input.GetKey(KeyCode.Space))
		{
			if(Time.time > lastTimeShot + miliSecondsFire/speedUp)
			{
				CreateBullet(this.transform.eulerAngles,this.transform.position + shipBody.GetRelativeVector(new Vector2(0,0.5f)).ToVector3(4));
				lastTimeShot = Time.time;
			}
		}
	}

	public void onPowerUp(string powerUpName)
	{
		if(powerUpName.Contains("Storm"))
		{
			shotBulletStorm();
		}
		else if(powerUpName.Contains("Fast"))
		{
			speedUp = 1.5f;
			Invoke("SpeedUpToNormal",5.0f);
		}
		else if(powerUpName.Contains("Shield"))
		{
			createShield();
		}
	}
	public void EnableImmortality()
	{
		Immortal = true;
		Invoke ("DisableImmoratlity", this.immortalityTime);
		TransformManager.Instance.AddTransform(gameObject,new PingPongColorTransform(Color.black,Color.red,immortalityTime,5.0f,(x,color) => x.GetComponent<Renderer>().material.color = color));
		
	}

	private void shotBulletStorm()
	{
		for(int i =0;i<16;i++)
		{
			CreateBullet(this.transform.eulerAngles.AddToZ(22.5f*(i+1)),Quaternion.AngleAxis(22.5f*(i+1),new Vector3(0,0,1))* (shipBody.GetRelativeVector(new Vector2(0,0.5f)).ToVector3(4)) + this.transform.position);
		}
	}
	private void createShield()
	{
		for(int i =0;i<3;i++)
		{
			createShieldPart(120*(i+2));
		}
	}

	private void CreateBullet(Vector3 rotation,Vector3 position)
	{
		GameObject bullet = ObjectCreator.Instance.createBullet();
		bullet.GetComponent<Bullet>().SetTransform(rotation,position);
		bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0,200.0f*speedUp));
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Asteroid")
			OnHit (coll.gameObject);
		
	}
	private void OnHit(GameObject badGuy)
	{
		if(Immortal)
			return;

		SetInitialState();
		hitPoints--;
		UI.Instance.SetBar(hitPoints);
		if(hitPoints == 0)
		{
			PositionManager.Instance.objectToCheck.Remove(gameObject);
			Destroy(gameObject);
			UI.Instance.ShowEndGameMessage();
			return;
		}
		TransformManager.Instance.AddTransform(badGuy,new PingPongColorTransform(Color.black,new Color(0.8f,0.2f,0.2f),immortalityTime,3.0f,(x,color) => x.GetComponent<Renderer>().material.color = color));
		EnableImmortality();
	}



	private void SetInitialState()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		shipBody.velocity = Vector2.zero;
		shipBody.angularVelocity = 0;
	}
	private void DisableImmoratlity()
	{
		Immortal = false;
	}
	private void SpeedUpToNormal()
	{
		speedUp = 1.0f;
	}
	private void createShieldPart(float angle)
	{
		GameObject shieldPart = ObjectCreator.Instance.createCircle(5);
		shieldPart.GetComponent<Rigidbody2D>().isKinematic = true;
		shieldPart.transform.parent = transform;
		shieldPart.transform.position = Quaternion.AngleAxis(angle,new Vector3(0,0,1))* (shipBody.GetRelativeVector(new Vector2(0,1.1f)).ToVector3()) + this.transform.position;
		shieldPart.AddComponent<RotateAround>();
		shieldPart.AddComponent<DestroyMe>().ScheduleDestruction(4.0f);
	}




}

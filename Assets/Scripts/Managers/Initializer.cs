using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

	public GameObject[] powerUps;

	private int waveCount = 0;
	private GameObject fader;
	private static Initializer instance = null;
	
	public static Initializer Instance
	{
		get
		{
			if(instance==null)
				instance = GameObject.FindObjectOfType<Initializer>();
			return instance;
		}
	}
	public void Start()
	{
		TransformManager.Instance.AddTransform(GameObject.Find("NoiseBackGround"),new PingPongColorTransform(new Color(0.8f,0.8f,0.8f),Color.red,10.0f,1,(x,color) => x.GetComponent<Renderer>().material.color = color,true));
	}
	public void StartGame()
	{
		UI.Instance.HideEndGameMessage();
		CancelInvoke();
		CreateAndShowFader();

	}

	public void SpawnAsteroidWave()
	{
		Ship ship  = GameObject.FindObjectOfType<Ship>();
		if(ship!=null)
			ship.EnableImmortality();

		for(int i = 0;i<waveCount + 8;i++)
		{
			ObjectCreator.Instance.createAsteroid();
		}
		waveCount++;
	}


	private  void CreateAndShowFader()
	{
		fader = ObjectCreator.Instance.createProceduralGradient();
		
		Bounds cameraBounds = Camera.main.OrthographicBounds();
		Bounds objectBounds = fader.GetComponent<Renderer>().bounds;
		
		float scaleParam = cameraBounds.size.x/objectBounds.size.x;
		fader.transform.localScale*=scaleParam*2;
		TransformManager.Instance.AddTransform(fader,new LinearColorTransform(Color.clear,Color.black,0.2f,(obj,col) => obj.GetComponent<Renderer>().material.color = col));
		Invoke("HideAndDestroyFader",0.21f);

	}
	private  void HideAndDestroyFader()
	{
		TransformManager.Instance.AddTransform(fader,new LinearColorTransform(Color.black,Color.clear,0.2f,(obj,col) => obj.GetComponent<Renderer>().material.color = col));
		Invoke("DestroyFader",0.21f);
	}
	private void DestroyFader()
	{
		DestroyAllCurrentObjects();
		CreateInitialObjects();
		InvokeRepeating("InitializePowerUp",5.0f,25.0f);
		Time.timeScale = 1.0f;
		Destroy(fader);
	}
	private void DestroyAllCurrentObjects()
	{
		for(int i = PositionManager.Instance.objectToCheck.Count -1;i>= 0;i--)
		{
			Destroy(PositionManager.Instance.objectToCheck[i]);
		}
		PositionManager.Instance.objectToCheck = new System.Collections.Generic.List<GameObject>();
	}
	private void CreateInitialObjects()
	{
		SpawnAsteroidWave();
		ObjectCreator.Instance.createShip();
	}

	private void InitializePowerUp()
	{
		Bounds cameraBounds = Camera.main.OrthographicBounds();
		Vector3 randomPosition = HelperUtil.getRandomVector(cameraBounds.size.x*0.45f,cameraBounds.size.y*0.45f);
		GameObject powerUp = (GameObject)Instantiate(powerUps[UnityEngine.Random.Range(0,powerUps.Length)],randomPosition,Quaternion.identity);

		TransformManager.Instance.AddTransform(powerUp,new LinearColorTransform(Color.clear,Color.white,0.5f,(x,col) => x.GetComponent<SpriteRenderer>().color = col));
		TransformManager.Instance.AddTransform(powerUp,new LinearScaleTransform(new Vector3(0.25f,0.25f,0.25f),new Vector3(0.75f,0.75f,0.75f),0.5f));

		powerUp.GetComponent<DestroyMe>().ScheduleDestruction(10.0f);
		PositionManager.Instance.objectToCheck.Add(powerUp);
	}
	

}

using UnityEngine;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour {

	private static int totalAsteroidDestroyed = 0;
	private static List<Asteroid> activeAsteroid = new List<Asteroid>();
	private int timesHit = 0;

	public void Start()
	{
		activeAsteroid.Add(this);
	}

	public void Hit(MathLine2D hitLine)
	{
		CreateParticles(hitLine.pointB);

		if(timesHit == 2)
		{
			CountHit();
		}
		else
		{
			Split(hitLine);
		}
	}
	public void DestroyMe()
	{
		PositionManager.Instance.objectToCheck.Remove(gameObject);
		activeAsteroid.Remove(this);

		if(activeAsteroid.Count == 0)
			Initializer.Instance.SpawnAsteroidWave();

		Destroy(gameObject);


	}
	public Mesh GetMyMesh()
	{
		return GetComponent<MeshFilter>().mesh;
	}
	public void CountHit()
	{
		UI.Instance.AddToScroe(60/(timesHit+1));
		totalAsteroidDestroyed++;
		DestroyMe();
	}

	private void Split(MathLine2D hitLine)
	{
		UI.Instance.AddToScroe(60/(timesHit+1));
		Mesh currentMesh = GetMyMesh();
		Vector3[] realLifePoints = new Vector3[currentMesh.vertices.Length];
		for(int i = 0;i<realLifePoints.Length;i++)
		{
			realLifePoints[i] = transform.TransformPoint(currentMesh.vertices[i]);
		}
		List<Vector3> one =  new List<Vector3>();
		List<Vector3> two =  new List<Vector3>();
		HelperUtil.splitGroupOfPoint(hitLine,new List<Vector3>(realLifePoints), one, two);
		List<Vector3> inter = HelperUtil.getIntersectionsPoints(realLifePoints,hitLine.pointA,hitLine.pointB);
		if(inter.Count == 2)
		{
			timesHit++;
			DestroyMe();

			CreateSplitPart(one,inter);
			CreateSplitPart(two,inter);
		}
	}
	private void CreateSplitPart(List<Vector3> verts,List<Vector3> intersctionPoints)
	{
		verts.AddRange(intersctionPoints);
		GameObject asteroid = ObjectCreator.Instance.createAsteroid(verts.ToArray());

		//For smaller targets polygon colliders stops working
		if(asteroid.GetComponent<MeshFilter>().mesh.GetArea() < 0.0003f)
			asteroid.GetComponent<Asteroid>().CountHit();

		asteroid.GetComponent<Asteroid>().timesHit = timesHit;
		TransformManager.Instance.AddTransform(asteroid,new PingPongColorTransform(Color.black,new Color(0.5f,0.2f,0.2f),3.0f,3.0f,(x,color) => x.GetComponent<Renderer>().material.color = color));
	}

	private void CreateParticles(Vector3 position)
	{
		ParticleSystem particles = (ParticleSystem)Instantiate(MaterialHolder.Instance.explosionParticles,position,Quaternion.identity);
		particles.gameObject.AddComponent<DestroyMe>().ScheduleDestruction(1.05f);
	}
}

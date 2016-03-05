using UnityEngine;
using System.Collections.Generic;

public class ObjectCreator {
	private static ObjectCreator instance = null;
	public static ObjectCreator Instance
	{
		get
		{
			if(instance == null)
				instance = new ObjectCreator();
			return instance;
		}
	}

	private ObjectCreator()
	{
	}

	public GameObject createCircle(int sub)
	{
		GameObject toReturn = new GameObject("Circle");
		Mesh mesh = createMesh(sub,new SimpleConvexTris(),new CricleVerts());
		toReturn.AddComponent<MeshFilter>().mesh = mesh;
		toReturn.AddComponent<MeshRenderer>().material = MaterialHolder.Instance.solidMaterial;

		Rigidbody2D body = toReturn.AddComponent<Rigidbody2D>();
		body.gravityScale = 0;
		body.drag = 0;
		body.angularDrag = 0;
		body.isKinematic = true;

		toReturn.AddComponent<CircleCollider2D>();

		return toReturn;
	}

	public GameObject createShip()
	{
		GameObject toReturn = new GameObject("Ship");
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]{new Vector3(0,0.4f),new Vector3(0.15f,0),new Vector3(0.0f,0.05f),new Vector3(-0.15f,0)};
		mesh.triangles = new int[]{0,1,2,0,2,3};
		toReturn.AddComponent<MeshFilter>().mesh = mesh;
		toReturn.AddComponent<MeshRenderer>().material = MaterialHolder.Instance.solidMaterial;

		Rigidbody2D body = toReturn.AddComponent<Rigidbody2D>();
		body.gravityScale = 0;
		body.drag = 1.0f;
		body.angularDrag = 2.0f;
		
		PolygonCollider2D collider = toReturn.AddComponent<PolygonCollider2D>();
		collider.points = new Vector2[]{new Vector2(0,0.4f),new Vector2(0.15f,0),new Vector2(-0.15f,0)};
		toReturn.AddComponent<Ship>();

		PositionManager.Instance.objectToCheck.Add(toReturn);

		GameObject[] objectArray = GameObject.FindGameObjectsWithTag("Background");
		for(int i=0;i<objectArray.Length;i++)
			objectArray[i].GetComponent<ParalaxBackground>().SetConnectedGameObject(toReturn);  

		return toReturn;
	}
	public GameObject createBullet()
	{
		GameObject toReturn = new GameObject("Bullet");
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]{new Vector3(0.15f,0.00f),new Vector3(0.00f,0.15f),new Vector3(-0.15f,0.00f)};
		mesh.triangles = new int[]{0,1,2};
		toReturn.AddComponent<MeshFilter>().mesh = mesh;

		PolygonCollider2D collider = toReturn.AddComponent<PolygonCollider2D>();
		collider.points = System.Array.ConvertAll( mesh.vertices,x => x.ToVector2());
		collider.isTrigger = true;

		toReturn.AddComponent<MeshRenderer>().material = MaterialHolder.Instance.solidMaterial;
		toReturn.GetComponent<MeshRenderer>().material.renderQueue = 2001;

		Rigidbody2D body = toReturn.AddComponent<Rigidbody2D>();
		body.gravityScale = 0;
		toReturn.AddComponent<Bullet>();
		toReturn = AddTrailRender(toReturn);
		return toReturn;
	}
	public GameObject createMesh(Vector3[] verts)
	{
		GameObject toReturn = new GameObject("Asteroid");
		Mesh mesh = createMesh(new SimpleConvexTris(),verts);
		toReturn.AddComponent<MeshFilter>().mesh = mesh;
		toReturn.AddComponent<MeshRenderer>().material = MaterialHolder.Instance.solidMaterial;
		return toReturn;
	}
	public GameObject createRandomizedMesh(int sub)
	{
		GameObject toReturn = new GameObject("Asteroid");
		Mesh mesh = createMesh(sub,new SimpleConvexTris(),new QuickHull());
		toReturn.AddComponent<MeshFilter>().mesh = mesh;
		toReturn.AddComponent<MeshRenderer>().material = MaterialHolder.Instance.solidMaterial;
		return toReturn;
	}
	public GameObject createAsteroid()
	{
		GameObject toReturn = createRandomizedMesh(20);
		AddAsteroidComponents(toReturn);
		toReturn.transform.position = HelperUtil.getRandomVector(20,20);
		return toReturn;
	}
	public GameObject createAsteroid(Vector3[] verts)
	{
		GameObject toReturn = createMesh(verts);
		return AddAsteroidComponents(toReturn);
	}


	private GameObject AddAsteroidComponents(GameObject toAdd)
	{
		Rigidbody2D body = toAdd.AddComponent<Rigidbody2D>();
		body.gravityScale = 0;
		body.drag = 0;
		body.angularDrag = 0;
		body.AddForce(HelperUtil.getRandomVector(125,125).ToVector2());
		body.AddTorque(UnityEngine.Random.Range(-30,30));
		
		PolygonCollider2D collider = toAdd.AddComponent<PolygonCollider2D>();
		Mesh mesh = toAdd.GetComponent<MeshFilter>().mesh;
		collider.points = System.Array.ConvertAll( mesh.vertices,x => x.ToVector2());
		PositionManager.Instance.objectToCheck.Add(toAdd);
		
		toAdd.AddComponent<Asteroid>();
		return toAdd;
	}

	private GameObject AddTrailRender(GameObject toAdd)
	{
		TrailRenderer trail = toAdd.AddComponent<TrailRenderer>();
		trail.endWidth = 0;
		trail.startWidth = 0.2f;
		trail.material = MaterialHolder.Instance.trailMaterial;
		trail.time = 0.75f;

		return toAdd;
	}
	private Mesh createMesh(ITriangulator trisMaker,Vector3[] verts)
	{
		Mesh mesh = new Mesh();

		Vector3 center =CalculateCentroid(verts);
		System.Array.Sort(verts,( x,y) =>  GetAngle(x,center).CompareTo( GetAngle(y,center)));
		mesh.vertices = verts;
		mesh.triangles = trisMaker.getTriangles(mesh.vertices);
		mesh.RecalculateBounds();
		return mesh;
	}
	private Mesh createMesh(int sub,ITriangulator trisMaker,IConvexHull vertsMaker)
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertsMaker.getVerts(sub);
		mesh.triangles = trisMaker.getTriangles(mesh.vertices);
		mesh.RecalculateBounds();
		return mesh;
	}
	private GameObject createLineRender(Vector3[] verts)
	{
		GameObject lineRenderer = new GameObject("LineRender");
		LineRenderer line = lineRenderer.AddComponent<LineRenderer>();;
		line.SetVertexCount(verts.Length + 1);
		for(int i = 0;i<verts.Length;i++)
			line.SetPosition(i,verts[i]);
		line.SetPosition(verts.Length,verts[0]);
		line.material = MaterialHolder.Instance.solidMaterial;
		return lineRenderer;
	}
	public GameObject createProceduralGradient()
	{
		GameObject gradient = new GameObject("Gradient");
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]{new Vector3(-0.2f,0.2f),new Vector3(0.2f,0.2f),new Vector3(-0.2f,0.1f),new Vector3(0.2f,0.1f),new Vector3(-0.2f,-0.1f),new Vector3(0.2f,-0.1f),new Vector3(-0.2f,-0.2f),new Vector3(0.2f,-0.2f)};
		mesh.colors = new Color[]{Color.clear,Color.clear,Color.black,Color.black,Color.black,Color.black,Color.clear,Color.clear};
		int[] tris = new int[18];
		for(int i =0,j =0;i<6;i++,j+=3)
		{
			tris[j] = i%2==0?i:i+2;
			tris[j+1] = i+1;
			tris[j+2] = i%2==0?i+2:i;
		}
		mesh.triangles = tris;
		mesh.RecalculateBounds();
		gradient.AddComponent<MeshFilter>().mesh = mesh;
		gradient.AddComponent<MeshRenderer>().material = MaterialHolder.Instance.vertexColorMat;
		return gradient;
	}


	private Vector3 CalculateCentroid(Vector3[] points)
	{
		Vector3 temp = Vector3.zero;
		for (int i = 0; i < points.Length; i++)
		{
			temp+=points[i];
		}
		return temp/points.Length;
	}
	
	private  float GetAngle(Vector3 a,Vector3 center)
	{
		return (Mathf.Atan2 (a.y - center.y, a.x - center.x))*180/Mathf.PI;
	}


}

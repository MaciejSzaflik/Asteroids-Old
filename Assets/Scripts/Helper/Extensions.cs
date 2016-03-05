using UnityEngine;
using System.Collections;

public static class Extensions
{
	public static Bounds OrthographicBounds(this Camera camera)
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		return bounds;
	}
	
	public static Vector2 BoundsMin(this Camera camera)
	{
		return camera.transform.position.ToVector2() - camera.Extents();
	}
	
	public static Vector2 BoundsMax(this Camera camera)
	{
		return camera.transform.position.ToVector2() + camera.Extents();
	}
	
	public static Vector2 Extents(this Camera camera)
	{
		if (camera.orthographic)
			return new Vector2(camera.orthographicSize * Screen.width/Screen.height, camera.orthographicSize);
		else
		{
			return new Vector2();
		}
	}
	
	public static Vector2 ToVector2(this Vector3 vec3)
	{
		return new Vector2(vec3.x,vec3.y);
	}
	public static Vector3 ToVector3(this Vector2 vec2,float z = 0)
	{
		return new Vector3(vec2.x,vec2.y,z);
	}
	
	public static Vector3 AddToX(this Vector3 vec3,float add)
	{
		return new Vector3(vec3.x + add,vec3.y,vec3.z);
	}
	public static Vector3 AddToY(this Vector3 vec3,float add)
	{
		return new Vector3(vec3.x,vec3.y + add,vec3.z);
	}
	public static Vector3 AddToZ(this Vector3 vec3,float add)
	{
		return new Vector3(vec3.x,vec3.y ,vec3.z + add);
	}
	public static Vector3 SetX(this Vector3 vec3,float set)
	{
		return new Vector3(set,vec3.y,vec3.z);
	}
	public static Vector3 SetY(this Vector3 vec3,float set)
	{
		return new Vector3(vec3.x,set,vec3.z);
	}

	public static float GetArea(this Mesh mesh)
	{
		float area = 0.0f;
		if(mesh.triangles.Length%3 != 0 || mesh.triangles.Length < 3)
			return area;

		for(int i = 0;i<mesh.triangles.Length;i+=3)
			area+= Triangle.getAreaOfTriangle(mesh.vertices[mesh.triangles[i]],mesh.vertices[mesh.triangles[i+1]],mesh.vertices[mesh.triangles[i+2]]);

		return area;
	}

}

using UnityEngine;
using System.Collections.Generic;

public class HelperUtil {

	public static Vector3 getRandomVector(float xMul = 1,float yMul = 1)
	{
		return new Vector3(UnityEngine.Random.Range(-1.0f,1.0f)*xMul,UnityEngine.Random.Range(-1.0f,1.0f)*yMul);
	}
	public static Vector3[] getRandomVertArray(int num)
	{
		Vector3[] toReturn = new Vector3[num];
		for(int i=0;i<num;i++)
			toReturn[i] = HelperUtil.getRandomVector(0.9f,0.9f);
		return toReturn;
	}
	public static void splitGroupOfPoint(MathLine2D splitLine,Vector3[] group,out Vector3[] groupOne, out Vector3[] groupTwo)
	{
		List<Vector3> listOne = new List<Vector3>();
		List<Vector3> listTwo = new List<Vector3>();
		for(int i =0;i<group.Length;i++)
		{
			float value = splitLine.getY(group[i].x);
			if(value > group[i].y)
				listOne.Add(group[i]);
			else
				listTwo.Add(group[i]);
		}
		groupOne = listOne.ToArray();
		groupTwo = listTwo.ToArray();
	}

	public static void splitGroupOfPoint(MathLine2D splitLine,List<Vector3> group,List<Vector3> groupOne,List<Vector3> groupTwo)
	{
		for(int i =0;i<group.Count;i++)
		{
			float value = splitLine.getY(group[i].x);
			if(value > group[i].y)
				groupOne.Add(group[i]);
			else
				groupTwo.Add(group[i]);
		}
	}

	public static List<Vector3> getIntersectionsPoints (Vector3[] vertices,Vector2 p1,Vector2 p2)
	{
		List<Vector3> intersectionPoints = new List<Vector3> ();
		for (int i = 0; i < vertices.Length; i++)
		{
			bool wasInstersection = false;
			Vector2 point = segmentLineIntersection (p1, p2, vertices [i ], vertices [(i + 1)%vertices.Length], out wasInstersection);
			if (wasInstersection)
				intersectionPoints.Add (point.ToVector3());
		}
		return intersectionPoints;
	}
	
	public static Vector3 segmentLineIntersection(Vector2 p1,Vector2 p2,Vector2 p3,Vector2 p4,out bool intersect)
	{
		intersect = false;
		Vector2 iP = lineIntersection (p1, p2, p3, p4, out intersect);
		if(!intersect)
			return Vector2.zero;
		
		float minX = Mathf.Min (p3.x, p4.x);
		float maxX = Mathf.Max (p3.x, p4.x);
		float minY = Mathf.Min (p3.y, p4.y);
		float maxY = Mathf.Max (p3.y, p4.y);
		
		bool xCheck = (minX <= iP.x && iP.x <= maxX);
		bool yCheck = (minY <= iP.y && iP.y <= maxY);
		intersect = xCheck && yCheck;
		return iP;
		
	}
	public static Vector2 lineIntersection(Vector2 p1,Vector2 p2,Vector2 p3,Vector2 p4,out bool notPararel)
	{
		float devider = (p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x);
		notPararel = (devider != 0);
		if (!notPararel) {
			return Vector2.zero;
		}
		
		float xD = (p1.x * p2.y - p1.y * p2.x) * (p3.x - p4.x) - (p1.x - p2.x) * (p3.x * p4.y - p3.y * p4.x);
		float yD = (p1.x * p2.y - p1.y * p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x * p4.y - p3.y * p4.x);
		
		return new Vector2 (xD/devider,yD/devider);
	}
}

public class MathLine2D
{
	float a,b;
	bool fake;
	float xfakeValue;
	public Vector3 pointA,pointB;
	public MathLine2D(Vector3 pointOne,Vector3 pointTwo)
	{
		pointA = pointOne;
		pointB = pointTwo;
		fake = (pointOne.x == pointTwo.x);
		xfakeValue = pointOne.x;
		a = (pointTwo.y - pointOne.y)/(pointTwo.x - pointOne.x);
		b = -a*pointOne.x + pointOne.y;
	}
	public float getY(float x)
	{
		if(fake)
		{
			if(x > xfakeValue)
				return float.PositiveInfinity;
			else
				return float.NegativeInfinity;
		}
		else
			return a*x + b;
	}
	public float getDistanceOfPoint(Vector3 point)
	{
		return Mathf.Abs(-a*point.x + point.y + b)/Mathf.Sqrt(Mathf.Pow(a,2) + 1);
	}
}
public class Triangle
{
	public class Edge
	{
		Vector3 a;
		Vector3 b;
		public Edge(Vector3 aa, Vector3 bb)
		{
			a = aa;
			b = bb;
		}

		public MathLine2D getLine()
		{
			return new MathLine2D(a,b);
		}
	}
	Vector3 a;
	Vector3 b;
	Vector3 c;
	public Edge ab;
	public Edge bc;
	public Edge ca;

	public Triangle(Vector3 one,Vector3 two ,Vector3 tri)
	{
		this.a = one;
		this.b = two;
		this.c = tri;
		ab = new Edge(a,b);
		bc = new Edge(b,c);
		ca = new Edge(c,a);

	}
	public void DrawDebug()
	{
		Debug.DrawLine(a,b,Color.red,5);
		Debug.DrawLine(a,c,Color.red,5);
		Debug.DrawLine(b,c,Color.red,5);
	}
	public bool isPointInside(Vector3 point)
	{
		float denominator = (b.y - c.y)*(a.x - c.x) + (c.x - b.x)*(a.y-c.y);
		float aValue = ((b.y-c.y)*(point.x-c.x)+(c.x-b.x)*(point.y-c.y))/denominator;
		float bValue = ((c.y-a.y)*(point.x-c.x)+(a.x-c.x)*(point.y-c.y))/denominator;
		float cValue = 1 - aValue - bValue;
		return 0 <= aValue && aValue <= 1 && 0 <= bValue && bValue <= 1 && 0 <= cValue && cValue <= 1; 
	}
	public bool isPointInsideVer2(Vector3 P)
	{

		bool b0 = ( Vector3.Dot( new Vector3(P.x - a.x, P.y - a.y),new Vector3(a.y - b.y, b.x - a.x))) > 0;
		bool b1 = (Vector3.Dot(new Vector3(P.x - b.x, P.y - b.y) , new Vector3(b.y - c.y, c.x - b.x))) > 0;
		bool b2 = (Vector3.Dot(new Vector3(P.x - c.x, P.y - c.y) , new Vector3(c.y - a.y, a.x - c.x))) > 0;
		if((b0 == b1 && b1 == b2) != isPointInside(P))
			Debug.LogError((b0 == b1 && b1 == b2) + " " + isPointInside(P));
		return!(b0 == b1 && b1 == b2);
	}

	public static float getAreaOfTriangle(Vector3 a,Vector3 b,Vector3 c)
	{
		float one = a.x*(b.y - c.y);
		float two = b.x*(c.y - a.y);
		float three = c.x*(a.y - b.y);
		return Mathf.Abs(one*two*three/2);
	}


}

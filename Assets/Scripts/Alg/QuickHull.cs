using UnityEngine;
using System.Collections.Generic;

public class QuickHull : IConvexHull {

	public Vector3[] getVerts (int num)
	{
		List<Vector3> pointsList = new List<Vector3>(HelperUtil.getRandomVertArray(20));
		return quickHull(pointsList).ToArray();
	}

	private int pointLocation(Vector3 A, Vector3 B, Vector3 P) {
		float cp1 = (B.x-A.x)*(P.y-A.y) - (B.y-A.y)*(P.x-A.x);
		return (cp1>0)?1:-1;
	}
	
	private float distanceV(Vector3 A, Vector3 B, Vector3 C) {
		float ABx = B.x-A.x;
		float ABy = B.y-A.y;
		float num = ABx*(A.y-C.y)-ABy*(A.x-C.x);
		if (num < 0) num = -num;
		return num;
	}  
	
	private List<Vector3> quickHull(List<Vector3> points) {
		List<Vector3> convexHull = new List<Vector3>();
		if (points.Count < 3) return points;
		
		int minVector3 = -1, maxVector3 = -1;
		float minX = float.MaxValue;
		float maxX = float.MinValue;
		for (int i = 0; i < points.Count; i++) {
			if (points[i].x < minX) {
				minX = points[i].x;
				minVector3 = i;
			}
			if (points[i].x > maxX) {
				maxX = points[i].x;
				maxVector3 = i;      
			}
		}
		Vector3 A = points[minVector3];
		Vector3 B = points[maxVector3];
		convexHull.Add(A);
		convexHull.Add(B);
		points.Remove(A);
		points.Remove(B);
		
		List<Vector3> leftSet = new List<Vector3>();
		List<Vector3> rightSet = new List<Vector3>();
		
		for (int i = 0; i < points.Count; i++) {
			Vector3 p = points [i];
			if (pointLocation(A,B,p) == -1)
				leftSet.Add(p);
			else
				rightSet.Add(p);
		}
		hullSet(A,B,rightSet,convexHull);
		hullSet(B,A,leftSet,convexHull);
		
		return convexHull;
	}
	
	private void hullSet(Vector3 A, Vector3 B, List<Vector3> set, List<Vector3> hull) {
		int insertPosition = hull.IndexOf(B);
		if (set.Count == 0) return;
		if (set.Count == 1) {
			Vector3 p = set[0];
			set.Remove(p);
			hull.Insert(insertPosition,p);
			return;
		}
		float dist = float.MinValue;
		int furthestVector3 = -1;
		for (int i = 0; i < set.Count; i++) {
			Vector3 p = set [i];
			float distance  = distanceV(A,B,p);
			if (distance > dist) {
				dist = distance;
				furthestVector3 = i;
			}
		}
		Vector3 P = set[furthestVector3];
		set.RemoveAt(furthestVector3);
		hull.Insert(insertPosition,P);
		
		List<Vector3> leftSetAP = new List<Vector3>();
		for (int i = 0; i < set.Count; i++) {
			Vector3 M = set[i];
			if (pointLocation(A,P,M)==1) {
				leftSetAP.Add(M);
			}
		}
		
		List<Vector3> leftSetPB = new List<Vector3>();
		for (int i = 0; i < set.Count; i++) {
			Vector3 M = set[i];
			if (pointLocation(P,B,M)==1) {
				leftSetPB.Add(M);
			}
		}
		hullSet(A,P,leftSetAP,hull);
		hullSet(P,B,leftSetPB,hull);
	}


}

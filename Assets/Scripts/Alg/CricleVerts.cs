using UnityEngine;
using System.Collections;

public class CricleVerts : IConvexHull {

	public Vector3[] getVerts(int num)
	{
		Vector3[] verts = new Vector3[num];
		float step = Mathf.PI*2/num;
		for(int i = 0;i<num;i++)
			verts[i] = new Vector3(Mathf.Cos(i*step)*0.25f,Mathf.Sin(i*step)*0.25f);
		return verts;
	}
}

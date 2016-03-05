using UnityEngine;
using System.Collections;

public class SimpleConvexTris : ITriangulator {

	public int[] getTriangles (Vector3[] vertexs)
	{
		int num = vertexs.Length - 2;
		int[] toReturn = new int[num*3];
		for(int i = 0;i<num;i++)
		{
			toReturn[i*3] = i+2;
			toReturn[i*3 + 1] = i+1;
			toReturn[i*3 + 2] = 0;
		}
		return toReturn;

	}
}

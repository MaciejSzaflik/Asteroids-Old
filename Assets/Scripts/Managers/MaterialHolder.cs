using UnityEngine;
using System.Collections;

public class MaterialHolder : MonoBehaviour {

	public Material solidMaterial;
	public Material outlineMaterial;
	public Material vertexColorMat;
	public Material trailMaterial;

	public ParticleSystem explosionParticles;

	private static MaterialHolder instance = null;

	public static MaterialHolder Instance
	{
		get
		{
			if(instance==null)
				instance = GameObject.FindObjectOfType<MaterialHolder>();
			return instance;
		}
	}
}

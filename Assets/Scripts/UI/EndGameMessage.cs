using UnityEngine;
using System.Collections;

public class EndGameMessage : MonoBehaviour {

	public TextMesh text;
	private GameObject background;

	public void Show()
	{
		text.text = "YOU DIED";
		background = ObjectCreator.Instance.createProceduralGradient();

		Bounds cameraBounds = Camera.main.OrthographicBounds();
		Bounds objectBounds = background.GetComponent<Renderer>().bounds;
		
		float scaleParam = cameraBounds.size.x/objectBounds.size.x;
		background.transform.localScale*=scaleParam;
		background.transform.localScale = background.transform.localScale.SetY(background.transform.localScale.x*0.33f);
		TransformManager.Instance.AddTransform(text.gameObject,new LinearColorTransform(Color.clear,new Color(0.7f,0.1f,0.1f,1.0f),0.3f,(obj,col) => obj.GetComponent<TextMesh>().color = col));
		TransformManager.Instance.AddTransform(background,new LinearColorTransform(Color.clear,Color.black,0.3f,(obj,col) => obj.GetComponent<Renderer>().material.color = col));
		TransformManager.Instance.AddTransform(text.gameObject,new LinearScaleTransform(new Vector3(0.7f,0.7f,0.7f),Vector3.one,0.1f));

		background.transform.localPosition = Vector3.zero;
		text.transform.localPosition = Vector3.zero;
		transform.position = new Vector3(0,0,-1.0f);
	}
	public void Hide()
	{
		TransformManager.Instance.AddTransform(text.gameObject,new LinearColorTransform(new Color(0.7f,0.1f,0.1f,1.0f),Color.clear,0.3f,(obj,col) => obj.GetComponent<TextMesh>().color = col));
		TransformManager.Instance.AddTransform(background,new LinearColorTransform(Color.black,Color.clear,0.3f,(obj,col) => obj.GetComponent<Renderer>().material.color = col));
		Invoke("DestroyMe",0.4f);
	}

	private void DestroyMe()
	{
		Destroy(gameObject);
	}
}

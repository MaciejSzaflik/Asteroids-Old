using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public EndGameMessage endGamePrefab;
	public Button restartButton;
	public Image[] healthBar;
	public Text scoreText;
	public Image help;
	public Button exitButton;

	private bool helpIsShown = false;
	private int[] transformIds;
	private EndGameMessage shownMessage;
	private int _scoreValue = 0;
	private int scoreValue
	{
		get
		{
			return _scoreValue;
		}
		set
		{
			_scoreValue = value;
			scoreText.text = value.ToString();
		}
	}

	private static UI instance = null;
	
	public static UI Instance
	{
		get
		{
			if(instance==null)
				instance = GameObject.FindObjectOfType<UI>();
			return instance;
		}
	}
	public void Start()
	{
		if(Application.isWebPlayer || Application.isEditor)
		{
			exitButton.interactable = false;
			exitButton.GetComponent<Image>().color = Color.clear;
		}
		scoreValue = 0;
		RestartGame();
	}

	public void ShowEndGameMessage()
	{

		shownMessage = ((GameObject)Instantiate(endGamePrefab.gameObject)).GetComponent<EndGameMessage>();
		shownMessage.Show();
		restartButton.interactable = true;
		TransformManager.Instance.AddTransform(restartButton.gameObject,new LinearColorTransform(Color.clear,Color.white,0.2f,(obj, color) => obj.GetComponent<Image>().color = color));
	}
	public void HideEndGameMessage()
	{
		if(shownMessage!=null)
			shownMessage.Hide();
	}

	public void RestartGame()
	{
		Initializer.Instance.StartGame();
		restartButton.interactable = false;
		CreateBarTransforms();
		TransformManager.Instance.AddTransform(restartButton.gameObject,new LinearColorTransform(Color.white,Color.clear,0.2f,(obj, color) => obj.GetComponent<Image>().color = color));
		scoreValue = 0;
	}
	public void SetBar(int value)
	{
		for(int i=healthBar.Length - 1;i >= 0;i--)
		{
			if(i + 1 >value)
			{
				TransformManager.Instance.CancelTransform(transformIds[healthBar.Length - 1 - i]);
				healthBar[healthBar.Length - 1 - i].color = Color.black;
			}
		}
	}

	public void AddToScroe(int value)
	{
		scoreValue+=value;
	}

	public void ExitApp()
	{
		Application.Quit();
	}
	public void ShowHelp()
	{
		if(!helpIsShown)
		{
			helpIsShown = true;
			TransformManager.Instance.AddTransform(help.gameObject,new LinearColorTransform(Color.clear,Color.white,0.1f,(x,color) => x.GetComponent<Image>().color = color));
			Invoke("HideHelp",2.0f);
		}
	}
	private void HideHelp()
	{
		Invoke("EnableHelp",0.11f);
		TransformManager.Instance.AddTransform(help.gameObject,new LinearColorTransform(Color.white,Color.clear,0.1f,(x,color) => x.GetComponent<Image>().color = color));
	}
	private void EnableHelp()
	{
		helpIsShown = false;
	}

	private void CreateBarTransforms()
	{
		transformIds = new int[healthBar.Length];
		for(int i = 0;i<healthBar.Length;i++)
		{
			transformIds[i] = TransformManager.Instance.AddTransform(healthBar[i].gameObject,new PingPongColorTransform(new Color(0.7f,0.1f,0.1f),new Color(0.9f,0.0f,0.2f),5,3 + i,(x,color) => x.GetComponent<Image>().color = color,true));
		}
	}
}

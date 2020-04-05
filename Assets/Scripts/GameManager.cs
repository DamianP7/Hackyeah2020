using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindObjectOfType<GameManager>();
			return instance;
		}
	}

	public int gameTime;

	[SerializeField] Text pointsText;
	[SerializeField] Text timeText;
	[SerializeField] Color goodTime, endOfTime;
	[SerializeField] CrowdController crowdController;
	[SerializeField] TutorialController tutorialController;

	public int Points
	{
		get => points;
		set
		{
			points = value;
			pointsText.text = points.ToString();
		}
	}

	public float Timer
	{
		get => timer;
		set
		{
			timer = value;
			timeText.text = Mathf.CeilToInt(timer).ToString();
			timeText.color = Mathf.CeilToInt(timer) > 5 ? goodTime : endOfTime;
		}
	}

	int points;
	float timer;
	bool started;

	private void Start()
	{
		Points = 0;
		Timer = gameTime;
	}

	public void StartGame()
	{
		crowdController.gameObject.SetActive(true);
		Points = 0;
		Timer = gameTime;
		started = true;
	}

	private void Update()
	{
		if (started)
		{
			Timer -= Time.deltaTime;
			if (timer <= 0)
			{
				started = false;
				crowdController.gameObject.SetActive(false);
				tutorialController.gameObject.SetActive(true);
			}
		}
	}
}

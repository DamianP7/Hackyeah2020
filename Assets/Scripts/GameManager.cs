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

	[SerializeField] Text pointsText;

	public int Points
	{
		get => points;
		set
		{
			points = value;
			pointsText.text = points.ToString();
		}
	}
	int points;

	private void Start()
	{
		Points = 0;
	}

}

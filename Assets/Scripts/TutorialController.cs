﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
	public TutorialWindow[] windows;

	public GameObject nextButton, prevButton;

	int index = 0;


	private void Awake()
	{
		Open();
	}

	private void OnEnable()
	{
		Open();
	}

	public void Open()
	{
		index = 0;
		for (int i = 0; i < windows.Length; i++)
		{
			windows[i].gameObject.SetActive(i == index);
		}
		if (index == windows.Length - 1)
			nextButton.SetActive(false);

		if (index == 0)
			prevButton.SetActive(false);
	}

	public void Close()
	{
		GameManager.Instance.StartGame();
		gameObject.SetActive(false);
	}

	public void Next()
	{
		prevButton.SetActive(true);
		if (index < windows.Length - 1)
		{
			windows[index].gameObject.SetActive(false);
			index++;
			windows[index].gameObject.SetActive(true);
		}
		if (index == windows.Length - 1)
			nextButton.SetActive(false);
	}

	public void Prev()
	{
		nextButton.SetActive(true);
		if (index > 0)
		{
			windows[index].gameObject.SetActive(false);
			index--;
			windows[index].gameObject.SetActive(true);
		}
		if (index == 0)
			prevButton.SetActive(false);
	}
}

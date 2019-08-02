﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryPanel : MonoBehaviour
{
	public void LoadNextPanel(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void LoadLevel(string name)
	{
		LoadingScreen.NextLevelName = name;
		SceneManager.LoadScene("Loading Screen");
	}

	private void Update()
	{
		CheckInput();
	}

	private void CheckInput()
	{
		if (Input.anyKeyDown)
		{
			GetComponent<Button>().onClick.Invoke();
		}
	}
}
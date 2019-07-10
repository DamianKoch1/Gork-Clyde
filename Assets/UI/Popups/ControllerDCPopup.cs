using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDCPopup : MonoBehaviour
{
	private bool checkingForDisconnects = false;
	private bool isVisible = false;
	
	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if (!checkingForDisconnects)
		{
			CheckControllerCount();
		}
		else if (!isVisible)
		{
			CheckForDisconnects();		
		}
	}

	private void CheckControllerCount()
	{
		if (Input.GetJoystickNames().Length > 1)
		{
			checkingForDisconnects = true;
		}
	}

	private void CheckForDisconnects()
	{
		foreach (var name in Input.GetJoystickNames())
		{
			if (String.IsNullOrEmpty(name))
			{
				Show(true);
				return;
			}
		}
	}
	
	
	public void Show(bool show)
	{
		isVisible = show;
		if (!show)
		{
			Time.timeScale = 1;
		}
		else
		{
			Time.timeScale = 0;
		}
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(show);
		}
	}
}

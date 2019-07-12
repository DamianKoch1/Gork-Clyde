using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDCPopup : MonoBehaviour
{
	private bool checkingForDisconnects = false;
	private bool isVisible = false;
	

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

	/// <summary>
	/// Checks if at least 2 controllers are plugged in
	/// </summary>
	private void CheckControllerCount()
	{
		if (Input.GetJoystickNames().Length > 1)
		{
			checkingForDisconnects = true;
		}
	}

	/// <summary>
	/// Pauses game if ingame and shows popup when noticing joystick disconnection
	/// </summary>
	private void CheckForDisconnects()
	{
		foreach (var joystick in Input.GetJoystickNames())
		{
			if (String.IsNullOrEmpty(joystick))
			{
				Show(true);
				return;
			}
		}
	}
	
	/// <summary>
	/// Toggles Popup
	/// </summary>
	/// <param name="show"></param>
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerDCPopup : MonoBehaviour
{
	private bool checkingForDisconnects = false;
	private bool isVisible = false;

	private GameObject previousSelected;

	private float previousTimescale;
	
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
	/// Checks if a joystick has been disconnected
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
	/// Toggles Popup, pauses game if popup is visible
	/// </summary>
	/// <param name="show"></param>
	public void Show(bool show)
	{
		isVisible = show;
		if (show)
		{
			previousSelected = EventSystem.current.currentSelectedGameObject;
			previousTimescale = Time.timeScale;
			EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = previousTimescale;
			EventSystem.current.SetSelectedGameObject(previousSelected);
		}
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(show);
		}
	}
}

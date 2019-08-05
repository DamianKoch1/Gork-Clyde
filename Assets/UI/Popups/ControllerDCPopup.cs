using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Pauses game and shows if controller is disconnected
/// </summary>
public class ControllerDCPopup : MonoBehaviour
{
	private bool checkingForDisconnects = false;
	private bool isVisible = false;

	private bool previousCursorVisible;

	private GameObject previousSelected;

	private float previousTimescale;

	[SerializeField] 
	private GameObject backButton;

	private static bool useKeyboard = false;
	
	private void Update()
	{
		CheckControllerStates();
	}

	/// <summary>
	/// If not using keyboard, checks if 2 controllers are plugged in, if so starts checking if one is disconnected
	/// </summary>
	private void CheckControllerStates()
	{
		if (useKeyboard) return;
		CheckControllerCount();
		if (!checkingForDisconnects) return;
		if (isVisible) return;
		CheckForDisconnects();		
	}

	/// <summary>
	/// Checks if at least 2 controllers are plugged in, closes popup when controller is replugged if it was visible
	/// </summary>
	private void CheckControllerCount()
	{
		if (Input.GetJoystickNames().Length > 1)
		{
			checkingForDisconnects = true;
			if (isVisible)
			{
				Show(false);
			}
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
	/// Disables this popup
	/// </summary>
	public void UseKeyboard()
	{
		useKeyboard = true;
	}
	
	/// <summary>
	/// Toggles popup
	/// </summary>
	/// <param name="show"></param>
	public void Show(bool show)
	{
		isVisible = show;
		if (show)
		{
			OnPopupShown();
		}
		else
		{
			OnPopupHidden();
		}
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(show);
		}
	}

	/// <summary>
	/// Saves previous cursor state, selected button and timescale, enables cursor, selects backButton, pauses game
	/// </summary>
	private void OnPopupShown()
	{
		previousCursorVisible = Cursor.visible;
		Cursor.visible = true;
		previousSelected = EventSystem.current.currentSelectedGameObject;
		previousTimescale = Time.timeScale;
		EventSystem.current.SetSelectedGameObject(backButton);
		Time.timeScale = 0;
	}

	/// <summary>
	/// Restores previous cursor state, selected button and timescale
	/// </summary>
	private void OnPopupHidden()
	{
		Cursor.visible = previousCursorVisible;
		Time.timeScale = previousTimescale;
		EventSystem.current.SetSelectedGameObject(previousSelected);
	}
	
	
}

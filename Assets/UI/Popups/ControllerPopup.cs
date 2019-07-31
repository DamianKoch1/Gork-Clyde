using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerPopup : MonoBehaviour
{
	/// <summary>
	/// Doesnt appear on start if true, likely going into playerPrefs
	/// </summary>
	public static bool HasShown = false;

	private void Start()
	{
		if (!HasShown)
		{
			Show(true);
			HasShown = true;
		}
	}

	/// <summary>
	/// Shows popup
	/// </summary>
	/// <param name="show"></param>
	public void Show(bool show)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(show);
		}

		if (show)
		{
			EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
		}
	}
}

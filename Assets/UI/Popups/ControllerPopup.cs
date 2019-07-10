using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPopup : MonoBehaviour
{
	public static bool HasShown = false;

	private void Start()
	{
		if (!HasShown)
		{
			Show(true);
			HasShown = true;
		}
	}

	public void Show(bool show)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(show);
		}
	}
}

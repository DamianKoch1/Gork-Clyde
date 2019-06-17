﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraActivator : MonoBehaviour
{
	private TempCamera cam;
	private int playersInTrigger = 0;
	
	private void Start()
	{
		cam = GetComponentInParent<TempCamera>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!cam.CanActivateCam(other)) return;

		playersInTrigger++;
		if (playersInTrigger == 2)
		{
			cam.ActivateCamera();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!cam.CanActivateCam(other)) return;

		playersInTrigger--;
	}
}
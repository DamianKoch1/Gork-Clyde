using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Cinemachine;
using UnityEngine;

public class TempCamera : MonoBehaviour
{
	private GameObject cam;

	private void Start()
	{
		InitializeVariables();
	}

	private void InitializeVariables()
	{
		cam = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
		cam.SetActive(false);
	}

	public void ActivateCamera()
	{
		if (cam.activeSelf) return;

		cam.SetActive(true);
	}

	public void DeactivateCamera()
	{
		if (!cam.activeSelf) return;

		cam.SetActive(false);
	}

	public bool CanActivateCam(Collider other)
	{
		if (other.isTrigger) return false;
		if (other.GetComponent<Player>()) return true;
		return false;
	}
}
using System;
using UnityEngine;

public class SingleplayerCameraDeactivator : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger) return;
		if (other.GetComponent<Clyde>())
		{
			CameraBehaviour.clydeCamEnabled = false;
		}

		if (other.GetComponent<Gork>())
		{
			CameraBehaviour.gorkCamEnabled = false;
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.isTrigger) return;
		if (other.GetComponent<Clyde>())
		{
			CameraBehaviour.clydeCamEnabled = true;
		}

		if (other.GetComponent<Gork>())
		{
			CameraBehaviour.gorkCamEnabled = true;
		}
	}
}

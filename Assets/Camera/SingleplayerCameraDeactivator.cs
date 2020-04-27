using System;
using UnityEngine;

/// <summary>
/// Deactivates respective players ability to focus camera on them while he is in this trigger
/// </summary>
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

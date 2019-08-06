using Cinemachine;
using UnityEngine;

/// <summary>
/// Activates when both players enter small trigger, disables if one leaves outer trigger
/// </summary>
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

	/// <summary>
	/// Enables this camera.
	/// </summary>
	public void ActivateCamera()
	{
		if (cam.activeSelf) return;

		cam.SetActive(true);
	}

	/// <summary>
	/// Disables this camera.
	/// </summary>
	public void DeactivateCamera()
	{
		if (!cam.activeSelf) return;

		cam.SetActive(false);
	}

	
	/// <summary>
	/// Checks if collider can activate camera (is a player)
	/// </summary>
	/// <param name="other">collider to check</param>
	/// <returns></returns>
	public bool CanActivateCam(Collider other)
	{
		if (other.isTrigger) return false;
		if (other.GetComponent<Player>()) return true;
		return false;
	}
}
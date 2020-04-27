using Cinemachine;
using UnityEngine;

/// <summary>
/// Activates when both players enter inner trigger, deactivates if one leaves outer trigger
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
}
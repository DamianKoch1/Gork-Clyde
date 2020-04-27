using UnityEngine;

/// <summary>
/// Activates Camera if 2 players are in trigger
/// </summary>
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
        if (other.isTrigger) return;
        if (!other.GetComponent<Player>()) return;

		playersInTrigger++;
		if (playersInTrigger == 2)
		{
			cam.ActivateCamera();
		}
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.isTrigger) return;
        if (!other.GetComponent<Player>()) return;

        playersInTrigger--;
	}
}
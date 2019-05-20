using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger) return;
		if (other.GetComponent<Player>())
		{
			other.GetComponent<Player>().Respawn();
		}
	}
}
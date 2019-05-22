using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Vector3 spawnpoint = Vector3.zero;
		if (other.isTrigger) return;
		if (other.GetComponent<Clyde>())
		{
			spawnpoint = Spawnpoint.CLYDE_SPAWN;
		}
		if (other.GetComponent<Gork>())
		{
			spawnpoint = Spawnpoint.GORK_SPAWN;
		}
		other.GetComponent<Player>().Respawn(spawnpoint);
	}
}
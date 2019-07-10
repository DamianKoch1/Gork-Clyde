using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        other.GetComponent<Respawning>()?.Respawn();
    }
}
using UnityEngine;

/// <summary>
/// Makes objects with Respawning component respawn when entering
/// </summary>
public class DeathZone : MonoBehaviour
{
    /// <summary>
    /// Makes objects with Respawning component respawn when entering
    /// </summary>
    /// <param name="other">collider to check for Respaning component</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        other.GetComponent<Respawning>()?.Respawn();
    }
}
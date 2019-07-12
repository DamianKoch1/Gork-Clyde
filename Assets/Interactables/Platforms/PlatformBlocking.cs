using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to prevent platforms squashing objects below
/// </summary>
public class PlatformBlocking : MonoBehaviour
{
    private MovingPlatform platform;

    private int blockingObjCount = 0;

    private void Start()
    {
        platform = GetComponentInParent<MovingPlatform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanBlock(other)) return;

        platform.Blocked();
        blockingObjCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!CanBlock(other)) return;
        blockingObjCount--;
        if (blockingObjCount == 0)
        {
            platform.Unblocked();
        }

    }

    /// <summary>
    /// Checks if collider is able to block platform
    /// </summary>
    /// <param name="other">collider to check</param>
    /// <returns>returns true if collider can block, false otherwise</returns>
    private bool CanBlock(Collider other)
    {
        if (other.isTrigger) return false;
        if (other.GetComponent<Player>()) return true;
        if (other.CompareTag("pushable")) return true;
        return false;
    }
}

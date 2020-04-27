using UnityEngine;

/// <summary>
/// Used to prevent platforms squashing objects below it
/// </summary>
public class PlatformBlockChecker : MonoBehaviour
{
    private MovingPlatform platform;

    private int blockingObjCount = 0;

    private void Start()
    {
        platform = GetComponentInParent<MovingPlatform>();
    }

    /// <summary>
    /// Blocks platform if other can block
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!CanBlock(other)) return;

        platform.Block();
        blockingObjCount++;
    }

    /// <summary>
    /// Unblocks platform if other was last blocking object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!CanBlock(other)) return;
        blockingObjCount--;
        if (blockingObjCount == 0)
        {
            platform.Unblock();
        }

    }

    /// <summary>
    /// Checks if collider is able to block platform
    /// </summary>
    /// <param name="other">Collider to check</param>
    /// <returns>Returns true if collider is player/pushable, false otherwise</returns>
    private bool CanBlock(Collider other)
    {
        if (other.isTrigger) return false;
        if (other.GetComponent<Player>()) return true;
        if (other.CompareTag("pushable")) return true;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlocking : MonoBehaviour
{
    private MovingPlatform platform;

    private int blockingObjCount = 0;

    [SerializeField]
    private string enteredMessage, exitedMessage;

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

    private bool CanBlock(Collider other)
    {
        if (other.isTrigger) return false;
        if (other.GetComponent<Player>()) return true;
        if (other.CompareTag("pushable")) return true;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlocking : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private int blockingObjCount = 0;

    [SerializeField]
    private string enteredMessage, exitedMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (!CanBlock(other)) return;

        target.SendMessage(enteredMessage);
        blockingObjCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!CanBlock(other)) return;

        blockingObjCount--;
        if (blockingObjCount == 0)
        {
            target.SendMessage(exitedMessage);
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

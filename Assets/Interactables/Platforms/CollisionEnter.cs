using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollisionEnter : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private int blockingObjCount = 0;

    [SerializeField]
    private string message, message2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("player") || other.CompareTag("pushable"))
        {
            target.SendMessage(message);
            blockingObjCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag("player") || other.CompareTag("pushable"))
        {
            blockingObjCount--;
            if (blockingObjCount == 0)
            {
                target.SendMessage(message2);
            }
        }
    }
}

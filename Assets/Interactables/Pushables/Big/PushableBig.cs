using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBig : Pushable
{

    [SerializeField]
    private float gorkPushDistance = 0.7f;
    
    /// <summary>
    /// Enables gork to push this object while in trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        var gork = other.GetComponent<Gork>();
        if (!gork) return;

        gork.pushedObj = gameObject;
    }

    /// <summary>
    /// Prevents gork from pushing this object after leaving it
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        var gork = other.GetComponent<Gork>();
        if (!gork) return;
        if (gork.pushing) return;
        
        gork.pushedObj = null;
    }

    public Vector3 GetClosestPushPosition(Vector3 objPosition)
    {
        Vector3[] gorkPushPositions = new Vector3[6];
        
        gorkPushPositions[0] = transform.position + transform.up * gorkPushDistance;
        gorkPushPositions[1] = transform.position - transform.up * gorkPushDistance;
        gorkPushPositions[2] = transform.position + transform.right * gorkPushDistance;
        gorkPushPositions[3] = transform.position - transform.right * gorkPushDistance;
        gorkPushPositions[4] = transform.position + transform.forward * gorkPushDistance;
        gorkPushPositions[5] = transform.position - transform.forward * gorkPushDistance;
        
        Vector3 closestPosition = gorkPushPositions[0];

        foreach (var pos in gorkPushPositions)
        {
            if (Vector3.Distance(objPosition, pos) < Vector3.Distance(objPosition, closestPosition))
            {
                closestPosition = pos;
            }
        }

        return closestPosition;
    }
    
}

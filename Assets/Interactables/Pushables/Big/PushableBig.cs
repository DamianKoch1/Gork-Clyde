using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBig : Pushable
{

    [SerializeField]
    private float gorkPushDistance = 2f;
    
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
        if (gork.isPushing) return;
        
        gork.pushedObj = null;
        gameObject.layer = 0;
    }

    public Vector3 GetClosestPushPosition(Vector3 objPosition)
    {
        Vector3[] gorkPushPositions = new Vector3[6];
        var transformPos = transform.position;
        var up = transform.up;
        var right = transform.right;
        var forward = transform.forward;
        
        gorkPushPositions[0] = transformPos + up * gorkPushDistance;
        gorkPushPositions[1] = transformPos - up * gorkPushDistance;
        gorkPushPositions[2] = transformPos + right * gorkPushDistance;
        gorkPushPositions[3] = transformPos - right * gorkPushDistance;
        gorkPushPositions[4] = transformPos + forward * gorkPushDistance;
        gorkPushPositions[5] = transformPos - forward * gorkPushDistance;
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBig : Pushable
{

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

}

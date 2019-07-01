using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBig : Pushable
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        var gork = other.GetComponent<Gork>();
        if (!gork) return;

        gork.pushedObj = gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        var gork = other.GetComponent<Gork>();
        if (!gork) return;
        if (!gork.pushedObj) return;

        gork.pushedObj = null;
    }

}

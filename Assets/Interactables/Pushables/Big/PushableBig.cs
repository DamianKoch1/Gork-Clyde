using UnityEngine;

public class PushableBig : Pushable
{
    /// <summary>
    /// Enables gork to push this object while in trigger
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        var gork = other.GetComponent<Gork>();
        if (!gork) return;

        gork.pushing.pushedObj = gameObject;
    }

    /// <summary>
    /// Prevents gork from pushing this object after leaving it
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        var gork = other.GetComponent<Gork>();
        if (!gork) return;
        if (gork.pushing.isPushing) return;
        
        gork.pushing.pushedObj = null;
        gameObject.layer = 0;
    }

  
    
}

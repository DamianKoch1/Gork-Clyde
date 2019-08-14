using UnityEngine;

public class PushableSmall : Pushable
{
    /// <summary>
    /// Enables clyde to push this object while in trigger
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        var clyde = other.GetComponent<Clyde>();
        if (!clyde) return;

        clyde.pushing.pushedObj = gameObject;
    }

    /// <summary>
    /// Prevents clyde from pushing this object after leaving it
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        var clyde = other.GetComponent<Clyde>();
        if (!clyde) return;
        if (clyde.pushing.isPushing) return;
        
        clyde.pushing.pushedObj = null;
        gameObject.layer = 0;
    }
}

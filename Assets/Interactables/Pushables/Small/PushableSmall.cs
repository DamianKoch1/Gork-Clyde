using UnityEngine;

public class PushableSmall : Pushable
{
    /// <summary>
    /// Enables players with "weak" push behaviours to push this object while in trigger, "strong" ones should push it just by walking, adjust mass accordingly
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        var pushBehaviour = other.GetComponent<PushBehaviour>();
        if (!pushBehaviour) return;
        if (pushBehaviour.canPushBigObjects) return;

        pushBehaviour.pushedObj = gameObject;
    }

    /// <summary>
    /// Prevents players with "weak" push behaviours from pushing this object after leaving it
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        var pushBehaviour = other.GetComponent<PushBehaviour>();
        if (!pushBehaviour) return;
        if (pushBehaviour.canPushBigObjects) return;
        if (pushBehaviour.isPushing) return;

        pushBehaviour.pushedObj = null;
        gameObject.layer = 0;
    }
}

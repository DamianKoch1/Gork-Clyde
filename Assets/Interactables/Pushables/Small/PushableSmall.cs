using UnityEngine;

public class PushableSmall : Pushable
{
    private int touchedPlayerCount = 0;

    /// <summary>
    /// Sets isPushed to true if player collides with this
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.isTrigger) return;
        if (!other.transform.GetComponent<Player>()) return;

        touchedPlayerCount++;
        isPushed = true;
    }

    /// <summary>
    /// Sets isPushed to false if player leaves collision with this
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionExit(Collision other)
    {
        if (other.collider.isTrigger) return;
        if (!other.transform.GetComponent<Player>()) return;

        touchedPlayerCount--;
        if (touchedPlayerCount == 0)
        {
            isPushed = false;
        }
    }

    
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

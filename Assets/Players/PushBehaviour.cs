using System;
using UnityEngine;

/// <summary>
/// Enables players to push objects
/// </summary>
public class PushBehaviour : MonoBehaviour
{
    private FixedJoint fixedJoint;

    public bool canPushBigObjects;

    [HideInInspector]
    public GameObject pushedObj;

    [HideInInspector]
    public bool isPushing;

    private Player player;

    private Animator anim;

    private Rigidbody rb;

    /// <summary>
    /// Add listener to e.g. change walk animation while pushing
    /// </summary>
    public Action onPushStarted;
    
    /// <summary>
    /// Add listener to revert changes above when stopping push
    /// </summary>
    public Action onPushStopped;
    
    public void Initialize(Animator _anim, Player _player, Rigidbody _rb)
    {
        anim = _anim;
        player = _player;
        rb = _rb;
    }
    
    protected void Update()
    {
        if (isPushing)
        {
            CheckIfStillPushing();
        }
    }
    
    /// <summary>
    /// Stops pushing if fixed joint broke or if falling
    /// </summary>
    private void CheckIfStillPushing()
    {
        if (!fixedJoint)
        {
            StopPushing();
        }
        else if (!player.wasGrounded)
        {
            StopPushing();
        }
    }
    
    /// <summary>
    /// Sets up pushing: adds fixed joint, changes movement, starts push animation
    /// </summary>
    public void StartPushing()
    {
        if (GetComponent<ThrowBehaviour>()?.IsCarryingObject() == true) return;
        if (isPushing) return;
        if (fixedJoint) return;
        if (!pushedObj) return;

        anim.SetBool("push", true);
        isPushing = true;
        Rigidbody objectRb = pushedObj.GetComponent<Rigidbody>();
        AlignToPushableSide();
        onPushStarted?.Invoke();
        pushedObj.GetComponent<Pushable>().isPushed = true;
        AddFixedJoint(objectRb);
    }

    /// <summary>
    /// Adds fixed joint connecting Gork with target, adapts breakForce to target mass
    /// </summary>
    /// <param name="target">RigidBody to connect joint with</param>
    private void AddFixedJoint(Rigidbody target)
    {
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = target;
        fixedJoint.breakForce = rb.mass * 400;
    }

    /// <summary>
    /// Reverts movement, destroys fixedJoint if still there, stops push animation
    /// </summary>
    public void StopPushing()
    {
        if (fixedJoint)
        {
            Destroy(fixedJoint);
        }

        if (pushedObj)
        {
            pushedObj.GetComponent<Pushable>().isPushed = false;
            pushedObj = null;
        }

        anim.SetBool("push", false);
        isPushing = false;
        onPushStopped?.Invoke();
    }

    /// <summary>
    /// Aligns self to closest side of pushed object
    /// </summary>
    private void AlignToPushableSide()
    {
        var pos = pushedObj.GetComponent<Pushable>().GetClosestPushPosition(rb.position);
		
        pos.y = rb.position.y;
        transform.position = pos;

        Vector3 lookAt = pushedObj.transform.position;
        lookAt.y = pos.y;
        transform.LookAt(lookAt);
    }

    /// <summary>
    /// Sets AngleToPushable parameter depending on motion angle to forward vector
    /// </summary>
    /// <param name="motion">Current movement motion</param>
    public void UpdateLegs(Vector3 motion)
    {
        var angle = Vector3.SignedAngle(transform.forward, motion.normalized, Vector3.up);
        while (angle < 0) angle += 360;
        anim.SetFloat("AngleToPushable", angle);
    }
}

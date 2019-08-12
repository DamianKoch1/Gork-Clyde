using UnityEngine;

/// <summary>
/// Handles everything pushing related
/// </summary>
public class Pushing : MonoBehaviour
{
    private FixedJoint fixedJoint;

    [HideInInspector]
    public GameObject pushedObj;

    [HideInInspector]
    public bool isPushing;

    private PlayerState state;
    
    private Animator anim;

    private Rigidbody rb;

    public delegate void OnPushStarted();
    /// <summary>
    /// Add listener to e.g. change walk animation while pushing
    /// </summary>
    public OnPushStarted onPushStarted;
    
    public delegate void OnPushStopped();
    /// <summary>
    /// Add listener to revert changes above when stopping push
    /// </summary>
    public OnPushStopped onPushStopped;
    
    public void Initialize(Animator _anim, PlayerState _state, Rigidbody _rb)
    {
        anim = _anim;
        state = _state;
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
        else if (!state.wasGrounded)
        {
            StopPushing();
        }
    }
    
    /// <summary>
    /// Sets up pushing: adds fixed joint, changes movement, starts push animation
    /// </summary>
    public void StartPushing()
    {
        if (GetComponent<Throwing>()?.IsCarryingObject() == true) return;
        if (isPushing) return;
        if (fixedJoint) return;
        if (!pushedObj) return;

        anim.SetBool("push", true);
        isPushing = true;
        Rigidbody objectRb = pushedObj.GetComponent<Rigidbody>();
        AlignToPushableSide();
        onPushStarted();
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
        onPushStopped();
    }

    /// <summary>
    /// Makes this object teleport in front of and look at closest pushable side
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

    public void UpdateLegs(Vector3 motion)
    {
        print(Vector3.Dot((pushedObj.transform.position - rb.position).normalized, (motion*transform.right).normalized));
    }
}

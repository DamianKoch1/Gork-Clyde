using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Carryable))]
public class Clyde : Player
{
    public static string XAxis = "ClydeHorizontal", ZAxis = "ClydeVertical", JumpButton = "ClydeJump";

    /// <summary>
    /// reference to gork, reenabling collision with when cancelling throw
    /// </summary>
    [SerializeField]
    private GameObject gork;

    /// <summary>
    /// starts after being thrown, can't get picked up while > 0
    /// </summary>
    private float pickupCooldown = 0;

    protected override void Start()
    {
        base.Start();
        InitializeInputs(XAxis, ZAxis, JumpButton);
    }

    
    
    protected override void Update()
    {
        base.Update();
        CheckIfOnGork();
    }

    /// <summary>
    /// Makes gork pick clyde up if clyde stands on him, prevents jumping on top of gork to reach unintended heights
    /// </summary>
    private void CheckIfOnGork()
    {
        if (pickupCooldown != 0) return;
        if (!groundedInfo.transform) return;
        var _gork = groundedInfo.transform.GetComponent<Gork>();
        if (_gork?.isPushing == true) return;
        _gork?.throwing.PickUp(gameObject);
    }

    public IEnumerator DecreasePickupCooldown()
    {
        pickupCooldown = 0.5f;
        while (pickupCooldown > 0)
        {
            pickupCooldown -= Time.deltaTime;
            yield return null;
        }
        pickupCooldown = 0;
    }

    /// <summary>
    /// Called when thrown
    /// </summary>
    public void RestartPickupCooldown()
    {
        StopCoroutine(DecreasePickupCooldown());
        StartCoroutine(DecreasePickupCooldown());
    }

    /// <summary>
    /// Clyde can exit gork if he wants, adds small forward force
    /// </summary>
    public void CancelThrow()
    {
        transform.SetParent(null, true);
        ResetMotion();
        canMove = true;
        GetComponent<Carryable>().isHeld = false;
        anim.SetTrigger("throwCancelled");
        gork.GetComponent<Gork>().anim.SetTrigger("cancelthrow");
        rb.isKinematic = false;
        Physics.IgnoreCollision(GetComponent<Collider>(), gork.GetComponent<Collider>(), false);
        rb.AddForce((transform.forward + transform.up) * 5, ForceMode.VelocityChange);
        RestartPickupCooldown();
    }
}
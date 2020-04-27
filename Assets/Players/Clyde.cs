using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Carryable))]
[RequireComponent(typeof(AirstreamAffected))]
public class Clyde : Player
{
    public static string XAxis = "ClydeHorizontal";
    public static string ZAxis = "ClydeVertical";
    public static string JumpButton = "ClydeJump";
    public static string ClydeInteract = "ClydeInteract";
    public static string ClydeCam = "ClydeCam";

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
    
    protected override void CheckInput()
    {
        base.CheckInput();

        if (Input.GetButtonDown(ClydeInteract))
        {
            if (pushBehaviour.pushedObj)
            {
                pushBehaviour.StartPushing();
            }
        }

        if (Input.GetButtonUp(ClydeInteract))
        {
            if (pushBehaviour.isPushing)
            {
                pushBehaviour.StopPushing();
            }
        }
    }
    
    

    /// <summary>
    /// Makes Gork pick Clyde up if clyde stands on him, prevents jumping on top of Gork to reach unintended heights
    /// </summary>
    private void CheckIfOnGork()
    {
        if (pickupCooldown != 0) return;
        if (!groundedInfo.transform) return;
        var _gork = groundedInfo.transform.GetComponent<Gork>();
        if (_gork?.pushBehaviour.isPushing == true) return;
        _gork?.throwBehaviour.PickUp(gameObject);
    }

    /// <summary>
    /// Sets pickupCooldown to 0.5s and decreases it, can't be picked up while > 0
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreasePickupCooldown()
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
    /// Clyde can exit gork using jump button, adds small forward force and detaches him from gork
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
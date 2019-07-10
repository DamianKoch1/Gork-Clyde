using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Player
{
    public static string XAxis = "ClydeHorizontal", ZAxis = "ClydeVertical", JumpButton = "ClydeJump";

    [SerializeField]
    private GameObject gork;

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


    private void CheckIfOnGork()
    {
        if (pickupCooldown != 0) return;
        if (!groundedInfo.transform) return;
        var _gork = groundedInfo.transform.GetComponent<Gork>();
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

    public void RestartPickupCooldown()
    {
        StopCoroutine(DecreasePickupCooldown());
        StartCoroutine(DecreasePickupCooldown());
    }

    public void CancelThrow()
    {
        transform.SetParent(null, true);
        ResetMotion();
        canMove = true;
        anim.SetTrigger("throwCancelled");
        gork.GetComponent<Gork>().anim.SetTrigger("cancelthrow");
        rb.isKinematic = false;
        Physics.IgnoreCollision(GetComponent<Collider>(), gork.GetComponent<Collider>(), false);
        rb.AddForce((transform.forward + transform.up) * 5, ForceMode.VelocityChange);
        RestartPickupCooldown();
    }
}
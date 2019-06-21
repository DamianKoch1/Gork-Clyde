using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Player
{
    public static string XAXIS = "ClydeHorizontal", ZAXIS = "ClydeVertical", JUMPBUTTON = "ClydeJump";

    [HideInInspector]
    public GameObject gork;

    private float pickupCooldown = 0;

    protected override void Start()
    {
        base.Start();
        InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
    }

    protected override void Update()
    {
        base.Update();
        CheckIfOnGork();
    }

    protected override void SetSpawnPoint()
    {
        Spawnpoint.CLYDE_SPAWN = rb.position;
    }

    public override void Respawn()
    {
        base.Respawn();
        rb.MovePosition(Spawnpoint.CLYDE_SPAWN);
    }

    private void CheckIfOnGork()
    {
        if (pickupCooldown != 0) return;
        if (!groundedInfo.transform) return;
        var gork = groundedInfo.transform.GetComponent<Gork>();
        if (!gork) return;
        if (gork.PickUp(gameObject)) return;

        Vector3 force = gork.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position;
        force.y = 0;
        force.Normalize();
        force += Vector3.up;
        rb.AddForce(force * 5, ForceMode.VelocityChange);
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
        rb.isKinematic = false;
        Physics.IgnoreCollision(GetComponent<Collider>(), gork.GetComponent<Collider>(), false);
        rb.AddForce((transform.forward + transform.up) * 5, ForceMode.VelocityChange);
        RestartPickupCooldown();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Player
{
    public static string XAXIS = "ClydeHorizontal", ZAXIS = "ClydeVertical", JUMPBUTTON = "ClydeJump";

    [HideInInspector]
    public GameObject gork;

    protected override void Start()
    {
        base.Start();
        InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
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

    public void CancelThrow()
    {
        transform.SetParent(null, true);
        ResetMotion();
        canMove = true;
        anim.SetTrigger("throwCancelled");
        rb.velocity = new Vector3(0, 1, 1);
        rb.isKinematic = false;
        Physics.IgnoreCollision(GetComponent<Collider>(), gork.GetComponent<Collider>(), false);
    }
}
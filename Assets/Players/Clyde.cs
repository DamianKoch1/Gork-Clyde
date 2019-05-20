using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Clyde : Player
{
    public static string XAXIS = "ClydeHorizontal", ZAXIS = "ClydeVertical", JUMPBUTTON = "ClydeJump";

    [HideInInspector]
    public GameObject gork;

    private void Start()
    {
        base.Start();
       
        InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
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
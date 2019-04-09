using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clide : Player
{
    [HideInInspector]
    public bool thrown = false;


    protected override void InitializeInputs()
    {
        xAxis = "Horizontal2";
        zAxis = "Vertical2";
        jumpButton = "Jump2";
    }

    protected override void SetVelocity()
    {
        if (thrown == false)
        {
            rb.velocity = (motion+ GetComponent<AirstreamAffected>().airstreamMotion) * Time.deltaTime * 60;
        }
        else if (isGrounded())
        {
            thrown = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.useGravity = false;
        }
    }
}
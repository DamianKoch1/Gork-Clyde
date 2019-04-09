using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clide : Player
{
   
    protected override void InitializeInputs()
    {
        xAxis = "Horizontal2";
        zAxis = "Vertical2";
        jumpButton = "Jump2";
    }

    protected override void setVelocity()
    {
        rb.velocity = (motion+ GetComponent<AirstreamAffected>().airstreamMotion) * Time.deltaTime * 60;
    }
}
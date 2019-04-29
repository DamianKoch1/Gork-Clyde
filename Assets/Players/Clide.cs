using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clide : Player
{
   

    protected override void InitializeInputs()
    {
        xAxis = "ClideHorizontal";
        zAxis = "ClideVertical";
        jumpButton = "ClideJump";
    }
    
    
    
//    protected override void SetVelocity()
//    {
//        if (GetComponent<AirstreamAffected>().inAirstream == false && Input.GetButton(jumpButton) && glideFallSpeed != 0)
//        {
//            motion.y = Mathf.Max(motion.y, -glideFallSpeed);
//        }
//        rb.MovePosition(transform.position + (motion + GetComponent<AirstreamAffected>().airstreamMotion) * Time.deltaTime);
//    }

    
}
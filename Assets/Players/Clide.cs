using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clide : Player
{
    [SerializeField]
    private float glideFallSpeed = 1f;


    protected override void InitializeInputs()
    {
        xAxis = "Horizontal2";
        zAxis = "Vertical2";
        jumpButton = "Jump2";
    }

    protected override void SetVelocity()
    {
        if (GetComponent<AirstreamAffected>().inAirstream == false && Input.GetButton(jumpButton) && glideFallSpeed != 0)
        {
            motion.y = Mathf.Max(motion.y, -glideFallSpeed);
        }
        rb.velocity = (motion + GetComponent<AirstreamAffected>().airstreamMotion) * Time.deltaTime * 60;
    }

    
}
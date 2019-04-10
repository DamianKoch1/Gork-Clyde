using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clide : Player
{
    [HideInInspector]
    public bool thrown = false;
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
        if (thrown == false)
        {
            if (GetComponent<AirstreamAffected>().inAirstream == false && Input.GetButton(jumpButton))
            {
                motion.y = Mathf.Max(motion.y, -glideFallSpeed);
            }
            rb.velocity = (motion+ GetComponent<AirstreamAffected>().airstreamMotion) * Time.deltaTime * 60;
            
        }
        else if (isGrounded() || GetComponent<AirstreamAffected>().inAirstream)
        {
            thrown = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.useGravity = false;
            motion = Vector3.zero;
        }
    }
}
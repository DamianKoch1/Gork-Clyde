using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clide : Player
{

    public Vector3 force;

    [SerializeField]
    private float maxForce;

    protected override void InitializeInputs()
    {
        xAxis = "ClideHorizontal";
        zAxis = "ClideVertical";
        jumpButton = "ClideJump";
    }

    private void Update()
    {
        base.Update();
        force *= 0.95f;
        if (force.magnitude < 1f) return;
        controller.Move(force * Time.deltaTime);
    }

    public void AddForce(Vector3 forceToAdd)
    {
        force += forceToAdd;
        if (force.magnitude > maxForce)
        {
            force = force.normalized * maxForce;
        }
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
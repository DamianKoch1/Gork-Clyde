using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected float speed, jumpHeight, fallSpeed;
    protected Vector3 motion;
    protected Rigidbody rb;
    protected string xAxis, zAxis, jumpButton;
    [SerializeField]
    protected Animator anim;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializeInputs();
        
    }
    protected virtual void FixedUpdate()
    {
        motion.x = Input.GetAxis(xAxis) * speed;
        motion.z = Input.GetAxis(zAxis) * speed;
        if (anim != null)
        {
            if (motion.x == 0 && motion.z == 0)
            {
                anim.SetBool("walking", false);
            }
            else
            {
                anim.SetBool("walking", true);
            }
        }

        if (isGrounded())
        {
            if (Input.GetButtonDown(jumpButton))
            {
                motion.y = jumpHeight;
                if (anim != null)
                {
                    anim.SetTrigger("jump");
                }
            }
            else
            {
                motion.y = 0;
                if (anim != null)
                {
                    anim.ResetTrigger("jump");
                }
            }
        }
        else
        {
            motion.y -= fallSpeed;
        }

        SetVelocity();
        Rotate();
        
    }
    protected virtual void SetVelocity()
    {
        rb.velocity = motion * Time.deltaTime * 60;
    }
    protected virtual void Rotate()
    {
        Vector3 lookAt;
        lookAt = transform.position + motion;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);
    }

    public bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
    }
    protected abstract void InitializeInputs();

}

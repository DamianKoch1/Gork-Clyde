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


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializeInputs();
        
    }
    protected virtual void FixedUpdate()
    {
        motion.x = Input.GetAxis(xAxis) * speed;
        motion.z = Input.GetAxis(zAxis) * speed;

        if (isGrounded())
        {
            if (Input.GetButtonDown(jumpButton))
            {
                motion.y = jumpHeight;
            }
            else
            {
                motion.y = 0;
            }
        }
        else
        {
            motion.y -= fallSpeed;
        }

        setVelocity();
        
    }
    protected virtual void setVelocity()
    {
        rb.velocity = motion * Time.deltaTime * 60;
    }

    public bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }
    protected abstract void InitializeInputs();

}

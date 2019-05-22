﻿using System.Collections;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected float speed, jumpHeight;
    protected Vector3 motion;
    [HideInInspector]
    public Rigidbody rb;
    protected string xAxis, zAxis, jumpButton;
    public Animator anim;
    private Camera cam;
    [SerializeField]
    private float maxGhostjumpDelay = 0.2f, jumpCooldown;
    private float ghostjumpTimer = 0f;
    [HideInInspector] 
    public bool canMove = true, inAirstream = false;
    private Vector3 parentPos;
    private ParticleSystem walkParticles;
    private bool wasGrounded = false, falling = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        walkParticles = GetComponentInChildren<ParticleSystem>();
        walkParticles.Stop();
        cam = Camera.main;
        StartCoroutine(CheckSpawnPoint());
    }

    protected void Update()
    {
        if (ghostjumpTimer > 0)
        {
            ghostjumpTimer = Mathf.Max(ghostjumpTimer - Time.deltaTime, 0);
        }
        if (jumpCooldown > 0)
        {
            jumpCooldown = Mathf.Max(jumpCooldown - Time.deltaTime, 0);
        }
        if (Input.GetButtonDown(jumpButton) && ghostjumpTimer > 0 && jumpCooldown == 0)
        {
            Jump();
        }
        
        //debug
        if (Input.GetButtonDown("DebugSonic"))
        {
            speed *= 2;
            jumpHeight *= 2;
        }
        if (Input.GetButtonDown("DebugUnSonic"))
        {
            speed /= 2;
            jumpHeight /= 2;
        }

        if (Input.GetButton("DebugAirjump"))
        {
            rb.AddForce(Vector3.up, ForceMode.VelocityChange);
        }
        //
        
    }

    private void Jump()
    {
        walkParticles.Stop();
        transform.SetParent(null, true);
        ghostjumpTimer = 0;
        jumpCooldown = 0.3f;
        rb.AddForce(jumpHeight*Vector3.up * Time.fixedDeltaTime*90, ForceMode.VelocityChange);
        anim.SetTrigger("jump");
        anim.ResetTrigger("land");
    }
    
    protected void FixedUpdate()
    {
        if (canMove)
        {
            motion.x = Input.GetAxis(xAxis);
            motion.z = Input.GetAxis(zAxis);
            anim.SetFloat("Blend", (Mathf.Abs(motion.x) + Mathf.Abs(motion.z)));
            motion = motion.normalized * speed;
            if (!inAirstream)
            {
                rb.AddForce(new Vector3(-rb.velocity.x, 0 , -rb.velocity.z)*Time.fixedDeltaTime*60, ForceMode.Acceleration);   
            }
            motion = ApplyCamRotation(motion);
            LookForward();
            MovePlayer();
        }
       
        if (IsGrounded())
        {
            if (!wasGrounded)
            {
                wasGrounded = true;
                falling = false;
                anim.SetBool("falling", false);
                anim.SetTrigger("land");
                anim.ResetTrigger("jump");
            }
            ghostjumpTimer = maxGhostjumpDelay;

            
            if (motion.x == 0 && motion.z == 0 && walkParticles.isPlaying)
            {
                walkParticles.Stop();
            }
            else if (!walkParticles.isPlaying)
            {
                    walkParticles.Play();
            }
        }
        else
        {
            wasGrounded = false;
            if (!falling && rb.velocity.y < -0.1f)
            {
                falling = true;
                anim.SetBool("falling", true);
            }
        }
    }

    public void Respawn(Vector3 spawnpoint)
    {
        ResetMotion();
        rb.velocity = Vector3.zero;
        rb.MovePosition(spawnpoint);
    }


    private IEnumerator CheckSpawnPoint()
    {
        while (true)
        {
            if (IsGrounded())
            {
                if (Physics.Raycast(rb.position, -Vector3.up,
                2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    SetSpawnPoint();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected abstract void SetSpawnPoint();
    
    protected virtual void MovePlayer()
    {
        //fixing player moving through walls when moving diagonally
        if (Physics.Raycast(rb.position - 0.7f*GetComponent<Collider>().bounds.extents.y*Vector3.up, motion.x * Vector3.right, 
        GetComponent<Collider>().bounds.extents.x*1.1f,Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.x = 0;
        }
        if (Physics.Raycast(rb.position - 0.7f*GetComponent<Collider>().bounds.extents.y*Vector3.up, motion.z * Vector3.forward, 
        GetComponent<Collider>().bounds.extents.x*1.1f,Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.z = 0;
        }
        rb.MovePosition(rb.position + motion * Time.fixedDeltaTime);
    }

  
    private void LookForward()
    {
        Vector3 position = transform.position;
        Vector3 lookAt = position + motion;
        lookAt.y = position.y;
        transform.LookAt(lookAt);
    }

    protected bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x / 2, -Vector3.up,
            out RaycastHit hitInfo, GetComponent<Collider>().bounds.extents.y - 0.1f, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
    }

    protected void InitializeInputs(string x, string z, string jump)
    {
        xAxis = x;
        zAxis = z;
        jumpButton = jump;
    }

    public void ResetMotion()
    {
        motion = Vector3.zero;
    }

    private Vector3 ApplyCamRotation(Vector3 vector)
    {
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();
        Vector3 rotatedVector = vector.x * camRight + vector.y * Vector3.up + vector.z * camForward;
        return rotatedVector;
    }
}

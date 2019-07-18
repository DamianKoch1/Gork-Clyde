using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for players
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected float speed, jumpHeight;

    /// <summary>
    /// speed multiplier if in air
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    private float airMovespeedPenalty = 0.9f;
    
    /// <summary>
    /// speed multiplier if thrown
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    private float throwMovespeedPenalty = 0.9f;
    [HideInInspector]
    public bool isThrown = false;
    
    protected Vector3 motion;
    [HideInInspector]
    public Rigidbody rb;
    protected string xAxis, zAxis, jumpButton;
    public Animator anim;

    //can jump while timer > 0, set to max when grounded, decreases otherwise
    [SerializeField]
    private float maxGhostjumpDelay = 0.2f;
    [HideInInspector]
    public float ghostjumpTimer = 0f;

    [HideInInspector]
    public bool canMove = true, inAirstream = false;
    private ParticleSystem walkParticles;
    private bool wasGrounded = false, falling = false;

    private List<Transform> collidingTransforms = new List<Transform>();

    protected delegate void SetMotion();

    protected SetMotion setMotion;

    protected RaycastHit groundedInfo;

    protected virtual void Start()
    {
        InitializeVariables();
    }


    protected virtual void Update()
    {
        CheckInput();
    }

    protected void FixedUpdate()
    {
        MovePlayer();

        UpdateState();
    }
    
    private void InitializeVariables()
    {
        rb = GetComponent<Rigidbody>();
        walkParticles = GetComponentInChildren<ParticleSystem>();
        setMotion = SetMotionDefault;
    }

    protected virtual void CheckInput()
    {
        if (Input.GetButtonDown(jumpButton))
        {
            if (wasGrounded)
            {
                Jump();
            }
            else if (ghostjumpTimer > 0)
            {
                Jump();
            }
        }

        //debug
        if (Input.GetButtonDown("DebugFast"))
        {
            Time.timeScale *= 2;
        }
        if (Input.GetButtonDown("DebugSlow"))
        {
            Time.timeScale /= 2;
        }

        if (Input.GetButton("DebugFly"))
        {
            rb.velocity = Vector3.up * 8;
        }
        //
    }

    private IEnumerator DecreaseGhostjumpTimer()
    {
        while (ghostjumpTimer > 0)
        {
            ghostjumpTimer -= Time.deltaTime;
            yield return null;
        }
        ghostjumpTimer = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collidingTransforms.Contains(other.transform)) return;
        collidingTransforms.Add(other.transform);
    }

    private void OnCollisionExit(Collision other)
    {
        if (!collidingTransforms.Contains(other.transform)) return;
        collidingTransforms.Remove(other.transform);
    }

    /// <summary>
    /// Standard moving function, rotates motion vector to camera view
    /// </summary>
    protected void SetMotionDefault()
    {
        float moveSpeed = speed;
        if (isThrown)
        {
            moveSpeed *= throwMovespeedPenalty;
        }
        else if (!wasGrounded)
        {
             moveSpeed *= airMovespeedPenalty;
        }
        motion.x = Input.GetAxis(xAxis);
        motion.z = Input.GetAxis(zAxis);
        if (motion.magnitude > 1)
        {
            motion = motion.normalized;
        }
        motion *= moveSpeed;
        motion = CameraBehaviour.ApplyCameraRotation(motion);
        LookForward();
    }


    private void Jump()
    {
        walkParticles.Stop();
        transform.SetParent(null, true);
        StopCoroutine(DecreaseGhostjumpTimer());
        ghostjumpTimer = 0;
        rb.AddForce(jumpHeight * Vector3.up * 1.6f, ForceMode.VelocityChange);
        anim.SetTrigger("jump");
        anim.ResetTrigger("land");
    }

    /// <summary>
    /// Setting animations / state bools
    /// </summary>
    private void UpdateState()
    {
        if (IsGrounded())
        {
            //landing?
            if (!wasGrounded)
            {
                ghostjumpTimer = maxGhostjumpDelay;
                falling = false;
                anim.SetBool("falling", false);
                anim.SetTrigger("land");
                anim.ResetTrigger("jump");
                if (isThrown)
                {
                    isThrown = false;
                }
            }
            wasGrounded = true;

            anim.SetFloat("Blend", ((Mathf.Abs(motion.x) + Mathf.Abs(motion.z))) / speed);

            //standing still?
            if (motion.magnitude < 0.1f)
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
            if (walkParticles.isPlaying)
            {
                walkParticles.Stop();
            }

            if (wasGrounded)
            {
                StartCoroutine(DecreaseGhostjumpTimer());
                wasGrounded = false;
            }
            //started falling?
            if (!falling)
            {
                if (rb.velocity.y < -0.1f)
                {
                    falling = true;
                    anim.SetBool("falling", true);
                }
            }
        }
    }

    /// <summary>
    /// Moves player based on motion, prevents walking through walls
    /// </summary>
    private void MovePlayer()
    {
        if (!canMove) return;

        setMotion();

        //applying "anti-force" to decrease sliding after exiting airstream
        if (!inAirstream)
        {
            rb.AddForce(new Vector3(-rb.velocity.x, 0, -rb.velocity.z) * Time.deltaTime * 60, ForceMode.Acceleration);
        }

        //fixing player moving through walls when moving diagonally
        var extents = GetComponent<Collider>().bounds.extents;
      
        if (Physics.Raycast(rb.position - 0.6f * extents.y * Vector3.up, motion.x * Vector3.right,
        extents.x * 1.1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.x = 0;
        }
        if (Physics.Raycast(rb.position - 0.6f * extents.y * Vector3.up, motion.z * Vector3.forward,
        extents.x * 1.1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.z = 0;
        }

        rb.MovePosition(rb.position + motion * Time.deltaTime);
    }

    /// <summary>
    /// looks into motion direction
    /// </summary>
    private void LookForward()
    {
        Vector3 position = transform.position;
        Vector3 lookAt = position + motion;
        lookAt.y = position.y;
        transform.LookAt(lookAt);
    }

    protected bool IsGrounded()
    {
        bool retval = Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x / 2, -Vector3.up,
        out groundedInfo, GetComponent<Collider>().bounds.extents.y - 0.1f, Physics.AllLayers,
        QueryTriggerInteraction.Ignore);
        if (collidingTransforms.Count == 0) retval = false;
        if (Mathf.Abs(rb.velocity.y) > 1) retval = false;
        return retval;
    }

    /// <summary>
    /// Sets up input axes/buttons to listen to
    /// </summary>
    /// <param name="x">x axis name</param>
    /// <param name="z">z axis name</param>
    /// <param name="jump">jump button name</param>
    protected void InitializeInputs(string x, string z, string jump)
    {
        xAxis = x;
        zAxis = z;
        jumpButton = jump;
    }

    public void ResetMotion()
    {
        rb.velocity = Vector3.zero;
        motion = Vector3.zero;
    }

  
}

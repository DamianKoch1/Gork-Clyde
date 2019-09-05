using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for players
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Pushing))]
public abstract class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float jumpHeight;
    
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
    public PlayerState state;
    
    [HideInInspector]
    public Pushing pushing;
  
    
    protected Vector3 motion;
    [HideInInspector]
    public Rigidbody rb;
    protected string xAxis, zAxis, jumpButton;
    
    [Header("References")]
    public Animator anim;

   

    protected delegate void SetMotion();

    protected SetMotion setMotion;

   

    protected virtual void Start()
    {
        InitializeVariables();
        
        InitializeComponents();
    }


    protected virtual void Update()
    {
        CheckInput();
        if (pushing.isPushing)
        {
            pushing.UpdateLegs(motion);
        }
    }

    protected void FixedUpdate()
    {
        MovePlayer();
        state.UpdateState(motion);
    }
    
    private void InitializeVariables()
    {
        rb = GetComponent<Rigidbody>();
        setMotion = SetMotionDefault;
    }

    private void InitializeComponents()
    {
        state = GetComponent<PlayerState>();
        state.Initialize(anim, rb);
        pushing = GetComponent<Pushing>();
        pushing.Initialize(anim, state, rb);
        pushing.onPushStarted = OnPushStarted;
        pushing.onPushStopped = OnPushStopped;
    }
    
    protected virtual void CheckInput()
    {
        if (!state.canMove) return;
        if (Input.GetButtonDown(jumpButton))
        {
            if (state.canJumpTimeframe > 0)
            {
                StartCoroutine(Jump());
            }
        }

     
    }

  
  
    /// <summary>
    /// Standard moving function, rotates motion vector to camera view
    /// </summary>
    protected void SetMotionDefault()
    {
        float finalSpeed = speed;
        if (state.isThrown)
        {
            finalSpeed *= throwMovespeedPenalty;
        }
        else if (!state.wasGrounded)
        {
             finalSpeed *= airMovespeedPenalty;
        }
        motion.x = Input.GetAxis(xAxis);
        motion.z = Input.GetAxis(zAxis);
        if (motion.magnitude > 1)
        {
            motion = motion.normalized;
        }
        motion *= finalSpeed;
        motion = CameraBehaviour.ApplyCameraRotation(motion);
        LookForward();
    }

    /// <summary>
    /// Disables rotating to motion vector
    /// </summary>
    private void SetMotionPushing()
    {
        motion.x = Input.GetAxis(xAxis);
        motion.z = Input.GetAxis(zAxis);

        if (motion.magnitude > 1)
        {
            motion.Normalize();
        }
		
        motion *= speed;
        motion = CameraBehaviour.ApplyCameraRotation(motion);
    }

    private void OnPushStarted()
    {
        ResetMotion();
        setMotion = SetMotionPushing;
    }

    private void OnPushStopped()
    {
        ResetMotion();
        setMotion = SetMotionDefault;
    }

    /// <summary>
    /// Starts adding downward force if jumpkey is no longer held during jump to enable smaller jumps
    /// </summary>
    /// <returns></returns>
    private IEnumerator Jump()
    {
        ResetMotion();
        transform.SetParent(null, true);
        StopCoroutine(state.DecreaseCanJumpTimer());
        state.canJumpTimeframe = 0;
        rb.AddForce(jumpHeight * Vector3.up * 1.6f, ForceMode.VelocityChange);
        anim.SetTrigger("jump");
        anim.ResetTrigger("land");
        bool jumpHeld = true;
        while (true)
        {
            if (rb.velocity.y < 0) yield break;
            if (jumpHeld)
            {
                if (!Input.GetButton(jumpButton))
                {
                    jumpHeld = false;
                }
            }
            else
            {
                rb.AddForce(-Vector3.up * 20, ForceMode.Acceleration);
            }
            yield return null;
        }
    }

    
   

    /// <summary>
    /// Moves player based on motion, prevents walking through walls
    /// </summary>
    private void MovePlayer()
    {
        if (!state.canMove) return;

        setMotion();

        //applying "anti-force" to decrease sliding after exiting airstream
        if (!state.inAirstream)
        {
            rb.AddForce(new Vector3(-rb.velocity.x, 0, -rb.velocity.z) * Time.deltaTime * 60, ForceMode.Acceleration);
        }

        //fixing player moving through walls when moving diagonally
        var extents = GetComponent<Collider>().bounds.extents;
      
        if (Physics.Raycast(rb.position - 0.6f * extents.y * Vector3.up, motion.x * Vector3.right,
        extents.x * 1.35f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.x = 0;
        }
        if (Physics.Raycast(rb.position - 0.6f * extents.y * Vector3.up, motion.z * Vector3.forward,
        extents.x * 1.35f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.z = 0;
        }

        rb.MovePosition(rb.position + motion * Time.deltaTime);
        anim.SetFloat("Blend", ((Mathf.Abs(motion.x) + Mathf.Abs(motion.z))) / speed);
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

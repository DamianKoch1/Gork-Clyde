using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for players
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PushBehaviour))]
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


    public PushBehaviour pushBehaviour { private set; get; }


    protected Vector3 motion;

    public Rigidbody rb { private set; get; }

    protected string xAxis;
    protected string zAxis;
    protected string jumpButton;

    [Header("References")]
    public Animator anim;


    protected Action setMotion;

    [SerializeField]
    private ParticleSystem walkParticles, landingParticles;

    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool isThrown = false;
    [HideInInspector]
    public bool inAirstream = false;
    [HideInInspector]
    public bool wasGrounded = false;

    private bool falling = false;

    [SerializeField]
    private float maxGhostjumpDelay = 0.2f;

    /// <summary>
    ///can jump while timer > 0, set to max when grounded, decreases otherwise
    /// </summary>
    [HideInInspector]
    public float canJumpTimeframe = 0f;

    public RaycastHit groundedInfo;

    private float highestFallVelocity = 0;

    [SerializeField]
    private float minLandingVFXFallSpeed = 15;

    private Vector3 landingVFXPosition;

    [Header("SFX")]

    [SerializeField]
    private AudioClip throwCollisionSFX;

    [SerializeField]
    private AudioClip landingSFX;

    [SerializeField]
    private AudioClip jumpSFX;

    [SerializeField]
    private AudioClip walkSFX;

    [SerializeField]
    private AudioClip idleSFX;

    [SerializeField]
    private AudioSource sfxAudioSource;

    [SerializeField]
    private AudioSource walkAudioSource;

    /// <summary>
    /// IsGrounded returns false if list is empty
    /// </summary>
    private List<Transform> collidingTransforms = new List<Transform>();


    protected virtual void Start()
    {
        InitializeVariables();

        InitializeComponents();
    }


    protected virtual void Update()
    {
        CheckInput();
        if (pushBehaviour.isPushing)
        {
            pushBehaviour.UpdateLegs(motion);
        }
    }

    protected void FixedUpdate()
    {
        MovePlayer();
        UpdateState(motion);
    }

    private void InitializeVariables()
    {
        rb = GetComponent<Rigidbody>();
        setMotion = SetMotionDefault;
        canJumpTimeframe = maxGhostjumpDelay;
        landingVFXPosition = landingParticles.transform.localPosition;
    }

    private void InitializeComponents()
    {
        pushBehaviour = GetComponent<PushBehaviour>();
        pushBehaviour.Initialize(anim, this, rb);
        pushBehaviour.onPushStarted = OnPushStarted;
        pushBehaviour.onPushStopped = OnPushStopped;
    }

    protected virtual void CheckInput()
    {
        if (!canMove) return;
        if (Input.GetButtonDown(jumpButton))
        {
            if (canJumpTimeframe > 0)
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
        if (isThrown)
        {
            finalSpeed *= throwMovespeedPenalty;
        }
        else if (!wasGrounded)
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
        StopCoroutine(DecreaseCanJumpTimer());
        canJumpTimeframe = 0;
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
        if (!canMove) return;

        setMotion?.Invoke();

        //applying "anti-force" to decrease sliding after exiting airstream
        if (!inAirstream)
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
        //if (rb.velocity.magnitude < motion.magnitude)
        //{
        //    rb.AddForce(motion * Time.deltaTime * 600, ForceMode.Acceleration);
        //}
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

    private void OnCollisionEnter(Collision other)
    {
        if (collidingTransforms.Contains(other.transform)) return;
        collidingTransforms.Add(other.transform);
        if (isThrown)
        {
            landingParticles.Stop();
            landingParticles.transform.position = other.contacts[0].point;
            landingParticles.transform.up = other.contacts[0].normal;
            landingParticles.Play();
            sfxAudioSource.PlayOneShot(throwCollisionSFX);
            isThrown = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!collidingTransforms.Contains(other.transform)) return;
        collidingTransforms.Remove(other.transform);
    }

    /// <summary>
    /// Starting animations / setting state bools accordingly
    /// </summary>
    public void UpdateState(Vector3 motion)
    {
        if (IsGrounded())
        {
            UpdateGroundedState(motion);
        }
        else
        {
            UpdateAirborneState();
        }

        //preventing unintended jumpboost after throw
        if (isThrown)
        {
            StopCoroutine(DecreaseCanJumpTimer());
            canJumpTimeframe = 0;
        }
    }

    /// <summary>
    /// Checks if currently landing/standing still
    /// </summary>
    private void UpdateGroundedState(Vector3 motion)
    {
        if (!wasGrounded)
        {
            OnLanding();
        }

        if (motion.magnitude < 0.1f)
        {
            OnStandingStill();
        }
        else if (!walkParticles.isPlaying)
        {
            OnWalking();
        }
        canJumpTimeframe = maxGhostjumpDelay;
    }

    /// <summary>
    /// Checks if falling/starting to fall or flying upwards
    /// </summary>
    private void UpdateAirborneState()
    {
        if (walkParticles.isPlaying)
        {
            walkParticles.Stop();
        }

        if (falling)
        {
            if (rb.velocity.y > 0.1f)
            {
                falling = false;
                anim.SetBool("falling", false);
                anim.SetTrigger("jump");
            }
            else if (rb.velocity.y < highestFallVelocity)
            {
                highestFallVelocity = rb.velocity.y;
            }
        }
        else
        {
            if (rb.velocity.y < -0.1f)
            {
                OnStartingFall();
            }
        }

        if (wasGrounded)
        {
            OnLeavingGround();
        }
    }

    /// <summary>
    /// Triggers land animation
    /// </summary>
    private void OnLanding()
    {
        falling = false;
        wasGrounded = true;
        anim.SetBool("falling", false);
        anim.ResetTrigger("jump");
        StopCoroutine(DecreaseCanJumpTimer());
        anim.SetTrigger("land");
        if (highestFallVelocity < -minLandingVFXFallSpeed)
        {
            landingParticles.Stop();
            landingParticles.transform.localPosition = landingVFXPosition;
            landingParticles.transform.up = Vector3.up;
            landingParticles.Play();
            sfxAudioSource.PlayOneShot(landingSFX);
        }
        highestFallVelocity = 0;
        if (isThrown)
        {
            isThrown = false;
        }
    }

    /// <summary>
    /// Stops walking dust vfx / sfx
    /// </summary>
    private void OnStandingStill()
    {
        walkParticles.Stop();
        if (walkAudioSource.clip != idleSFX)
        {
            walkAudioSource.Stop();
            walkAudioSource.clip = idleSFX;
            walkAudioSource.Play();
        }
    }

    /// <summary>
    /// Plays walking dust vfx / sfx
    /// </summary>
    private void OnWalking()
    {
        walkParticles.Play();
        if (walkAudioSource.clip != walkSFX)
        {
            walkAudioSource.Stop();
            walkAudioSource.clip = walkSFX;
            walkAudioSource.Play();
        }
    }

    /// <summary>
    /// Triggers fall animation
    /// </summary>
    private void OnStartingFall()
    {
        falling = true;
        anim.SetBool("falling", true);
    }

    /// <summary>
    /// If falling, starts decreasing timeframe in which player can jump to make slightly missed jump timings more forgiving
    /// </summary>
    private void OnLeavingGround()
    {
        wasGrounded = false;
        walkAudioSource.Stop();
        if (falling)
        {
            canJumpTimeframe = maxGhostjumpDelay;
            StartCoroutine(DecreaseCanJumpTimer());
        }
        else
        {
            StopCoroutine(DecreaseCanJumpTimer());
            canJumpTimeframe = 0;
            sfxAudioSource.PlayOneShot(jumpSFX);
        }
    }

    /// <summary>
    /// Checks if player is on ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        bool retval = Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x / 2, -Vector3.up,
        out groundedInfo, GetComponent<Collider>().bounds.extents.y - 0.1f, Physics.AllLayers,
        QueryTriggerInteraction.Ignore);
        if (collidingTransforms.Count == 0) retval = false;
        if (groundedInfo.transform)
        {
            if (!groundedInfo.transform.CompareTag("platform"))
            {
                if (Mathf.Abs(rb.velocity.y) > 0.5f) retval = false;
            }
        }
        return retval;
    }

    /// <summary>
    /// Preventing rare double jump glitches by spamming it
    /// </summary>
    /// <returns></returns>
    public IEnumerator DecreaseCanJumpTimer()
    {
        while (true)
        {
            canJumpTimeframe = Mathf.Max(canJumpTimeframe - Time.deltaTime, 0);
            yield return null;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(CheckSpawnPoint());
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
        cam = Camera.main;
        setMotion = SetMotionDefault;
    }

    protected virtual void CheckInput()
    {
        if (Input.GetButtonDown(jumpButton))
        {
            if (IsGrounded())
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
            speed *= 2;
            jumpHeight *= 2;
        }
        if (Input.GetButtonDown("DebugSlow"))
        {
            speed /= 2;
            jumpHeight /= 2;
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

    protected void SetMotionDefault()
    {
        motion.x = Input.GetAxis(xAxis);
        motion.z = Input.GetAxis(zAxis);
        if (motion.magnitude > 1)
        {
            motion = motion.normalized;
        }
        motion *= speed;
        motion = ApplyCamRotation(motion);
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
            }
            wasGrounded = true;

            anim.SetFloat("Blend", (Mathf.Abs(motion.x) + Mathf.Abs(motion.z)));

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


    public virtual void Respawn()
    {
        ResetMotion();
        rb.velocity = Vector3.zero;
    }


    public IEnumerator CheckSpawnPoint()
    {
        SetSpawnPoint();
        while (true)
        {
            if (IsGrounded())
            {
                //prevent spawning too close to edge
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
        if (Physics.Raycast(rb.position - 0.7f * GetComponent<Collider>().bounds.extents.y * Vector3.up, motion.x * Vector3.right,
        GetComponent<Collider>().bounds.extents.x * 1.1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.x = 0;
        }
        if (Physics.Raycast(rb.position - 0.7f * GetComponent<Collider>().bounds.extents.y * Vector3.up, motion.z * Vector3.forward,
        GetComponent<Collider>().bounds.extents.x * 1.1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            motion.z = 0;
        }

        rb.MovePosition(rb.position + motion * Time.deltaTime);
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
        bool retval = false;
        if (Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x / 2, -Vector3.up,
            out groundedInfo, GetComponent<Collider>().bounds.extents.y - 0.1f, Physics.AllLayers,
            QueryTriggerInteraction.Ignore)) retval = true;
        if (collidingTransforms.Count == 0) retval = false;
        if (Mathf.Abs(rb.velocity.y) > 1) retval = false;
        return retval;
    }

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

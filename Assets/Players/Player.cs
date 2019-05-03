using UnityEngine;

public abstract class Player : MonoBehaviour
{
    [SerializeField]
    protected float speed, jumpHeight;
    protected Vector3 motion;
    [HideInInspector]
    public Rigidbody rb;
    protected string xAxis, zAxis, jumpButton;
    [SerializeField]
    protected Animator anim;
    private Camera cam;
    [SerializeField]
    private float maxGhostjumpDelay = 0.2f, jumpCooldown;
    private float ghostjumpTimer = 0f;
    [HideInInspector] 
    public bool canMove = true, inAirstream = false;
    private RaycastHit hit;
    private Vector3 parentPos;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializeInputs();
        cam = Camera.main;
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
    }

    protected virtual void FixedUpdate()
    {
        if (IsGrounded())
        {
            ghostjumpTimer = maxGhostjumpDelay;
            if (anim != null)
            {
                anim.ResetTrigger("jump");
            }
        }

        if (canMove == true)
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
            if (Input.GetButtonDown(jumpButton) && ghostjumpTimer > 0 && jumpCooldown == 0)
            {
                transform.SetParent(null, true);
                ghostjumpTimer = 0;
                jumpCooldown = 0.3f;
                rb.AddForce(jumpHeight*Vector3.up * Time.deltaTime*90, ForceMode.VelocityChange);
                if (anim != null)
                {
                    anim.SetTrigger("jump");
                }
            }
            if (inAirstream == false)
            {
                rb.AddForce(new Vector3(-rb.velocity.x, 0 , -rb.velocity.z)*Time.deltaTime*60, ForceMode.Acceleration);   
            }
            motion = ApplyCamRotation(motion);
            MovePlayer();
            LookForward();
        }
    }


    protected virtual void MovePlayer()
    {
        rb.MovePosition(rb.position + motion * Time.deltaTime);
    }

  
    private void LookForward()
    {
        Vector3 position = transform.position;
        Vector3 lookAt = position + motion;
        lookAt.y = position.y;
        transform.LookAt(lookAt);
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x / 2, -Vector3.up,
            out hit, GetComponent<Collider>().bounds.extents.y - 0.1f, Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore);
    }
    protected abstract void InitializeInputs();

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

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
    private Camera cam;
    [SerializeField]
    private float maxGhostjumpDelay = 0.2f;
    private float ghostjumpTimer = 0f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializeInputs();
        cam = Camera.main;
    }

    protected virtual void FixedUpdate()
    {
        if (ghostjumpTimer > 0)
        {
            ghostjumpTimer = Mathf.Max(ghostjumpTimer - Time.deltaTime, 0);
        }
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
        if (IsGrounded())
        {        
            motion.y = 0;
            if (anim != null)
            {
                anim.ResetTrigger("jump");
            }
        }
        else
        {
            motion.y -= fallSpeed;
        }
        if (Input.GetButtonDown(jumpButton) && ghostjumpTimer > 0)
        {
            transform.SetParent(null, true);
            ghostjumpTimer = 0;
            motion.y = jumpHeight;
            if (anim != null)
            {
                anim.SetTrigger("jump");
            }
        }
        motion = ApplyCamRotation(motion);
        SetVelocity();
        LookForward();
        
    }


    protected virtual void SetVelocity()
    {
        rb.velocity = motion * Time.deltaTime * 60;
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
        if (!Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x / 2, -Vector3.up,
            out RaycastHit hitInfo, GetComponent<Collider>().bounds.extents.y - 0.1f, Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore)) return false;
        ghostjumpTimer = maxGhostjumpDelay;
        return true;

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

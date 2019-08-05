using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
   
    
    private Animator anim;

    private Rigidbody rb;
    
    private ParticleSystem walkParticles;

    [HideInInspector]
    public bool canMove = true,
    isThrown = false,
    inAirstream = false,
    wasGrounded = false;
    
    private bool falling = false;
    
    [SerializeField]
    private float maxGhostjumpDelay = 0.2f;
    
    /// <summary>
    ///can jump while timer > 0, set to max when grounded, decreases otherwise
    /// </summary>
    [HideInInspector]
    public float canJumpTimeframe = 0f;
    
    private List<Transform> collidingTransforms = new List<Transform>();
    
    public RaycastHit groundedInfo;
    
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


    public void Initialize(Animator _anim, Rigidbody _rb, ParticleSystem _walkParticles)
    {
        anim = _anim;
        rb = _rb;
        walkParticles = _walkParticles;
        canJumpTimeframe = maxGhostjumpDelay;
    }

    /// <summary>
    /// Starting animations / setting state bools accordingly
    /// </summary>
    public void UpdateState()
    {
        if (IsGrounded())
        {
            UpdateGroundedState();
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

    private void UpdateGroundedState()
    {
        if (!wasGrounded)
        {
            OnLanding();
        }

        if (rb.velocity.magnitude < 0.1f)
        {
            OnStandingStill();
        }
        else if (!walkParticles.isPlaying)
        {
            walkParticles.Play();
        }
        canJumpTimeframe = maxGhostjumpDelay;
    }

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
    
    private void OnLanding()
    {
        falling = false;
        wasGrounded = true;
        anim.SetBool("falling", false);
        anim.SetTrigger("land");
        anim.ResetTrigger("jump");
        StopCoroutine(DecreaseCanJumpTimer());
        if (isThrown)
        {
            isThrown = false;
        }
    }

    private void OnStandingStill()
    {
        walkParticles.Stop();
    }

    private void OnStartingFall()
    {
        falling = true;
        anim.SetBool("falling", true);
    }
    
    private void OnLeavingGround()
    {
        wasGrounded = false;
        if (falling)
        {
            canJumpTimeframe = maxGhostjumpDelay;
            StartCoroutine(DecreaseCanJumpTimer());
        }
    }
    
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

    public IEnumerator DecreaseCanJumpTimer()
    {
        while (true)
        {
            canJumpTimeframe = Mathf.Max(canJumpTimeframe - Time.deltaTime, 0);
            yield return null;
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all state related information/functionality of a player
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayerState : MonoBehaviour
{
    private Animator anim;

    private Rigidbody rb;
    
    [SerializeField]
    private ParticleSystem walkParticles, landingParticles;

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
    private AudioSource sfxAudioSource;
    
    [SerializeField]
    private AudioSource walkAudioSource;
    
    //-----------
    /// <summary>
    /// IsGrounded returns false if list is empty
    /// </summary>
    private List<Transform> collidingTransforms = new List<Transform>();
   
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
    //----------

    public void Initialize(Animator _anim, Rigidbody _rb)
    {
        anim = _anim;
        rb = _rb;
        canJumpTimeframe = maxGhostjumpDelay;
        landingVFXPosition = landingParticles.transform.position;
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

    /// <summary>
    /// Checks if currently landing/standing still
    /// </summary>
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
    /// Triggers land animation, toggles other states
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
            landingParticles.transform.position = landingVFXPosition;
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
        walkAudioSource.Stop();
    }
    
    /// <summary>
    /// Plays walking dust vfx / sfx
    /// </summary>
    private void OnWalking()
    {
        walkParticles.Play();
        walkAudioSource.Play();
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
            yield return new WaitForEndOfFrame();
        }
    }

    
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for pushables, can also be attached to gameobject to make it pushable, override OnTriggerEnter/Exit to decide which player can push
/// </summary>
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Pushable : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem pushedParticles, landingParticles;
    private Rigidbody rb;
    private Vector3 previousPosition, currentPosition;
    [HideInInspector]
    public bool isPushed;
    
    [SerializeField]
    private float playerPushDistance = 2f;

    [Header("SFX")] 
    
    [SerializeField]
    private AudioSource sfxAudioSource;
    
    [SerializeField]
    private AudioSource pushedAudioSource;
    
    private void Start()
    {
        InitializeVariables();
    }

    private void FixedUpdate()
    {
        UpdateState();
        UpdateLayer();
    }


    /// <summary>
    /// prevents big pushables getting pushed from just walking against it
    /// </summary>
    private void UpdateLayer()
    {
        if (isPushed)
        {
            gameObject.layer = 2;
        }
        else
        {
            gameObject.layer = 0;
        }
    }
    
    private void InitializeVariables()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = rb.position;
        currentPosition = previousPosition;
        sfxAudioSource = GetComponent<AudioSource>();
    }
    
    /// <summary>
    /// Plays vfx / sfx if moving
    /// </summary>
    private void UpdateState()
    {
        currentPosition = rb.position;
        pushedParticles.transform.position = currentPosition - Vector3.up * transform.localScale.y / 5;
        if (IsMoving())
        {
            EmitPushedParticles();
            if (!pushedAudioSource.isPlaying)
            {
                pushedAudioSource.Play();
            }
        }
        else if (pushedParticles.isPlaying)
        {
            pushedParticles.Stop();
            pushedAudioSource.Stop();
        }
        previousPosition = currentPosition;
    }

    /// <summary>
    /// Checks if pushable is moving
    /// </summary>
    /// <returns>Returns false if not pushed / falling / position unchanged, true otherwise</returns>
    private bool IsMoving()
    {
        if (!isPushed) return false;
        if (Mathf.Abs(rb.velocity.y) > 0.2f) return false;
        if (currentPosition == previousPosition) return false;
        return true;
    }

    /// <summary>
    /// Emits vfx in correct direction
    /// </summary>
    private void EmitPushedParticles()
    {
        Vector3 particleDirection = previousPosition - currentPosition;
        pushedParticles.transform.LookAt(pushedParticles.transform.position + particleDirection);
        if (!pushedParticles.isPlaying)
        {
            pushedParticles.Play();
        }
    }
    
    /// <summary>
    /// Returns closest pushable side to given player position
    /// </summary>
    /// <param name="playerPos">position of player</param>
    /// <returns></returns>
    public Vector3 GetClosestPushPosition(Vector3 playerPos)
    {
        Vector3[] pushPositions = new Vector3[6];
        var transformPos = transform.position;
        var up = transform.up;
        var right = transform.right;
        var forward = transform.forward;
        
        pushPositions[0] = transformPos + up * playerPushDistance;
        pushPositions[1] = transformPos - up * playerPushDistance;
        pushPositions[2] = transformPos + right * playerPushDistance;
        pushPositions[3] = transformPos - right * playerPushDistance;
        pushPositions[4] = transformPos + forward * playerPushDistance;
        pushPositions[5] = transformPos - forward * playerPushDistance;
        
        Vector3 closestPosition = pushPositions[0];

        foreach (var pos in pushPositions)
        {
            if (Vector3.Distance(playerPos, pos) < Vector3.Distance(playerPos, closestPosition))
            {
                closestPosition = pos;
            }
        }

        closestPosition.y = playerPos.y;
        return closestPosition;
    }
    
    /// <summary>
    /// Enables player to push this object while in trigger
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        var player = other.GetComponent<Pushing>();
        if (!player) return;

        player.pushedObj = gameObject;
    }

    /// <summary>
    /// Prevents player from pushing this object after leaving it
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        var player = other.GetComponent<Pushing>();
        if (!player) return;
        if (player.isPushing) return;
        
        player.pushedObj = null;
        gameObject.layer = 0;
    }
    
    //----------------
    /// <summary>
    /// Plays collision vfx when landing after throw/fall
    /// </summary>
    private List<Transform> collidingTransforms = new List<Transform>();
   
    private void OnCollisionEnter(Collision other)
    {
        if (collidingTransforms.Contains(other.transform)) return;
        if (collidingTransforms.Count == 0)
        {
            landingParticles.transform.position = other.contacts[0].point;
            landingParticles.transform.up = other.contacts[0].normal;
            landingParticles.Stop();
            landingParticles.Play();
            sfxAudioSource.Stop();
            sfxAudioSource.Play();
        }
        collidingTransforms.Add(other.transform);
    }

    private void OnCollisionExit(Collision other)
    {
        if (!collidingTransforms.Contains(other.transform)) return;
        collidingTransforms.Remove(other.transform);
    }
    //----------------
}

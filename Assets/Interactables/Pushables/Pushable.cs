using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pushable : MonoBehaviour
{
    private ParticleSystem pushedParticles;
    private Rigidbody rb;
    private Vector3 previousPosition, currentPosition;
    [HideInInspector]
    public bool isPushed;

    private void Start()
    {
        InitializeVariables();
    }

    private void FixedUpdate()
    {
        UpdateParticles();
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
        pushedParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        previousPosition = rb.position;
        currentPosition = previousPosition;
    }
    
    /// <summary>
    /// Shows vfx if pushed
    /// </summary>
    private void UpdateParticles()
    {
        currentPosition = rb.position;
        pushedParticles.transform.position = currentPosition - Vector3.up * transform.localScale.y / 5;
        if (ShowPushedParticles())
        {
            EmitPushedParticles();
        }
        else if (pushedParticles.isPlaying)
        {
            pushedParticles.Stop();
        }
        previousPosition = currentPosition;
    }

    /// <summary>
    /// Checks if vfx should be shown
    /// </summary>
    /// <returns>Returns false if not pushed / falling / position unchanged, true otherwise</returns>
    private bool ShowPushedParticles()
    {
        if (!isPushed) return false;
        if (Mathf.Abs(rb.velocity.y) > 0.1f) return false;
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
}

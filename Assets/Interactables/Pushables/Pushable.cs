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

    private void InitializeVariables()
    {
        pushedParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        previousPosition = rb.position;
        currentPosition = previousPosition;
    }

    private void FixedUpdate()
    {
        UpdateParticles();
    }

    private void UpdateParticles()
    {
        currentPosition = rb.position;
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

    private bool ShowPushedParticles()
    {
        if (!isPushed) return false;
        if (rb.velocity.y > 0.1f) return false;
        if (currentPosition == previousPosition) return false;
        return true;
    }

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

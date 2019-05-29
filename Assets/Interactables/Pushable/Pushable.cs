using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pushable : MonoBehaviour
{
     private ParticleSystem pushedParticles;
     private Rigidbody rb;
     private Vector3 previousPosition, currentPosition;
     protected bool isPushed;

     private void Start()
     {
          InitializeVariables();
     }

     private void InitializeVariables()
     {
          pushedParticles = GetComponentInChildren<ParticleSystem>();
          rb = GetComponent<Rigidbody>();
          previousPosition = rb.position;
          currentPosition = previousPosition;
     }

     private void FixedUpdate()
     {
          if (ShowPushedParticles())
          {
               PlayPushedParticles();
          }
          else if (pushedParticles.isPlaying)
          {
               pushedParticles.Stop();
          }
     }

     private bool ShowPushedParticles()
     {
          if (!isPushed) return false;
          if (rb.velocity.y > 0.1f) return false;
          currentPosition = rb.position;    
          if (currentPosition == previousPosition) return false;
          previousPosition = rb.position;
          return true;
     }

     private void PlayPushedParticles()
     {
          pushedParticles.transform.LookAt(pushedParticles.transform.position - (currentPosition - previousPosition));
          if (!pushedParticles.isPlaying)
          {
               pushedParticles.Play();
          }
     }
}

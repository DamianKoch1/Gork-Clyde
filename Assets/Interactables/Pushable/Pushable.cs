using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
     private ParticleSystem pushedParticles;
     private Rigidbody rb;
     private Vector3 previousPosition;
     private int touchedPlayerCount = 0;
     private bool isPushed = false;

     private void Start()
     {
          pushedParticles = GetComponentInChildren<ParticleSystem>();
          pushedParticles.Stop();
          rb = GetComponent<Rigidbody>();
          previousPosition = rb.position;
     }

     private void OnCollisionEnter(Collision other)
     {
          if (other.gameObject.CompareTag("player"))
          {
               touchedPlayerCount++;
               isPushed = true;
          }
     }

     private void OnCollisionExit(Collision other)
     {
          if (other.gameObject.CompareTag("player"))
          {
               touchedPlayerCount--;
               if (touchedPlayerCount == 0)
               {
                    isPushed = false;
                    pushedParticles.Stop();
               }
          }
     }

     private void FixedUpdate()
     {
          if (isPushed)
          {
               Vector3 currentPos = rb.position;
               if (currentPos != previousPosition && rb.velocity.y < 0.1f)
               {
                    pushedParticles.transform.LookAt(pushedParticles.transform.position - (currentPos - previousPosition));
                    if (!pushedParticles.isPlaying)
                    {
                         pushedParticles.Play();
                    }
               }
               else
               {
                    if (pushedParticles.isPlaying)
                    {
                         pushedParticles.Stop();
                    }
               }
               previousPosition = rb.position;
          }
     }
}

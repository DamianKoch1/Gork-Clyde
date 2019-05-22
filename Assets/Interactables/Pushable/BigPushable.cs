using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPushable : MonoBehaviour
{
     private ParticleSystem pushedParticles;
     private Rigidbody rb;
     private Vector3 previousPosition;
     [SerializeField]
     public bool isPushed = false;
     private float startMass;

     private void Start()
     {
          pushedParticles = GetComponentInChildren<ParticleSystem>();
          pushedParticles.Stop();
          rb = GetComponent<Rigidbody>();
          previousPosition = rb.position;
          startMass = rb.mass;
     }

  


     private void OnTriggerStay(Collider other)
     {
          if (other.GetComponent<Gork>())
          {
               if (Input.GetButtonDown(Gork.GORKINTERACT))
               {
                    other.GetComponent<Gork>().StartPushing(gameObject);
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

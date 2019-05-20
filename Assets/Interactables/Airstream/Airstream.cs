using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstream : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField]
    private float strength;
   
    void Start()
    {
        direction = transform.forward;
    }


    private void OnTriggerStay(Collider other)
    {
        var clyde = other.GetComponent<Clyde>();
        if (other.GetComponent<AirstreamAffected>()  || clyde)
        {
            other.GetComponent<Rigidbody>().AddForce(direction * strength * Time.deltaTime * 60, ForceMode.Acceleration);
            if (clyde)
            {
                if (!clyde.inAirstream)
                {
                    clyde.inAirstream = true;
                    if (!clyde.canMove)
                    {
                        clyde.CancelThrow();
                    }
                }
                other.transform.SetParent(null, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var clide = other.GetComponent<Clyde>();
        if (clide)
        {
            clide.inAirstream = false;
        }
    }

    public void OnButtonActivated()
    {
        ParticleSystem particles = transform.GetChild(0).GetComponent<ParticleSystem>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        BoxCollider bc = GetComponent<BoxCollider>();
        if (mr.enabled)
        {
            particles.Stop();
        }
        else
        {
            particles.Play();
        }
        mr.enabled = !mr.enabled;
        bc.enabled = !bc.enabled;
    }
}

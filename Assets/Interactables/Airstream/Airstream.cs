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


//    private void OnTriggerEnter(Collider other)
//    {
//        var clide = other.GetComponent<Clide>();
//        if (clide != null)
//        {
//            other.transform.SetParent(null, true);
//            clide.AddForce(direction * strength);
//        }
//    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<AirstreamAffected>() != null)
        {
             other.GetComponent<Rigidbody>().AddForce(direction * strength * Time.deltaTime * 60, ForceMode.Acceleration);
        }
        else
        {
            var clide = other.GetComponent<Clide>();
            if (clide != null)
            {
                if (clide.inAirstream == false)
                {
                    clide.inAirstream = true;
                }
                other.transform.SetParent(null, true);
                clide.AddForce(direction * strength / 10);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var clide = other.GetComponent<Clide>();
        if (clide != null)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstream : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField]
    private float strength;
   


    private void OnTriggerStay(Collider other)
    {
        var clyde = other.GetComponent<Clyde>();
        if (other.GetComponent<AirstreamAffected>())
        {
            AddAirstreamForce(other.GetComponent<Rigidbody>());
        }
        else if (clyde)
        {
            if (!clyde.inAirstream)
            {
                OnClydeAirstreamEntered(clyde);
            }
            AddAirstreamForce(clyde.GetComponent<Rigidbody>());
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        var clyde = other.GetComponent<Clyde>();
        if (clyde)
        {
            clyde.inAirstream = false;
        }
    }

    private void AddAirstreamForce(Rigidbody rb)
    {
        direction = transform.forward;
        rb.AddForce(direction * strength * Time.deltaTime * 60, ForceMode.Acceleration);
    }

    private void OnClydeAirstreamEntered(Clyde clyde)
    {
        clyde.inAirstream = true;
        if (!clyde.canMove)
        {
            clyde.CancelThrow();
        }
        clyde.gameObject.transform.SetParent(null, true);
    }
    
    
    public void OnButtonActivated()
    {
        ToggleAirstream();
    }

    private void ToggleAirstream()
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

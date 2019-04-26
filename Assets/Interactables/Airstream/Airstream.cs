using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstream : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField]
    private float strength;
    List<Collider> colliders = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
    }

   
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<AirstreamAffected>() != null)
        {
            if (colliders.Contains(other) == false)
            {
                colliders.Add(other);
            }
            AirstreamAffected airstreamAffected = other.GetComponent<AirstreamAffected>();
            airstreamAffected.airstreamMotion = direction * strength;
            airstreamAffected.inAirstream = true;
            other.transform.SetParent(null, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AirstreamAffected>() != null)
        {
            if (colliders.Contains(other)){
                colliders.Remove(other);
            }
            other.GetComponent<AirstreamAffected>().inAirstream = false;
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
            foreach (Collider collider in colliders)
            {
                collider.GetComponent<AirstreamAffected>().inAirstream = false;
            }
        }
        else
        {
            particles.Play();
        }
        mr.enabled = !mr.enabled;
        bc.enabled = !bc.enabled;
    }
}

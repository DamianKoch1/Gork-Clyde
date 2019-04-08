using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstream : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField]
    private float strength;
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<AirstreamAffected>() != null)
        {
            AirstreamAffected airstreamAffected = other.GetComponent<AirstreamAffected>();
            airstreamAffected.airstreamMotion = direction * strength;
            airstreamAffected.inAirstream = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AirstreamAffected>() != null)
        {
            AirstreamAffected airstreamAffected = other.GetComponent<AirstreamAffected>();
            airstreamAffected.inAirstream = false;
        }
    }



    public void OnButtonActivated()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        BoxCollider bc = GetComponent<BoxCollider>();
        mr.enabled = !mr.enabled;
        bc.enabled = !bc.enabled;
    }
}

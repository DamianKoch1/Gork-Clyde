﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Airstream : MonoBehaviour, IActivatable
{
    private Vector3 direction;
    [SerializeField]
    private float strength;

  
    [SerializeField]
    private ParticleSystem vfx1, vfx2;

  
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        if (!other.GetComponent<AirstreamAffected>()) return;

        AddAirstreamForce(other.GetComponent<Rigidbody>());
        var clyde = other.GetComponent<Clyde>();
        if (clyde)
        {
            if (!clyde.inAirstream)
            {
                OnClydeAirstreamEntered(clyde);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        var clyde = other.GetComponent<Clyde>();
        if (clyde)
        {
            clyde.inAirstream = false;
        }
    }

    /// <summary>
    /// Applies airstream force to a RigidBody.
    /// </summary>
    /// <param name="rb">RigidBody to apply force to</param>
    private void AddAirstreamForce(Rigidbody rb)
    {
        direction = transform.forward;
        rb.AddForce(direction * strength * Time.deltaTime * 60, ForceMode.Acceleration);
    }

    /// <summary>
    /// Used to detach Clyde from gork if he tries to carry Clyde through an airstream.
    /// </summary>
    /// <param name="clyde">Reference to Clyde</param>
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

    public void OnButtonDeactivated()
    {
        ToggleAirstream();
    }

    public void OnPlateActivated()
    {
        ToggleAirstream();
    }

    public void OnPlateExited()
    {
        ToggleAirstream();
    }

    /// <summary>
    /// Toggles airstream on or off.
    /// </summary>
    public void ToggleAirstream()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        BoxCollider bc = GetComponent<BoxCollider>();
        if (mr.enabled)
        {
            vfx1.Stop();
            vfx2.Stop();
        }
        else
        {
            vfx1.Play();
            vfx2.Play();
        }
        mr.enabled = !mr.enabled;
        bc.enabled = !bc.enabled;
    }
}

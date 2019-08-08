using System.Collections.Generic;
using UnityEngine;

public class Airstream : MonoBehaviour, IActivatable
{
    private Vector3 direction;
    [SerializeField]
    private float strength;

    [SerializeField]
    private bool activeAtStart = true;
    
    [SerializeField]
    private List<Ventilator> ventilators = new List<Ventilator>();

  
    private void Start()
    {
        SetStartState();
    }

    /// <summary>
    /// Toggles airstream on/off depending on activeAtStart
    /// </summary>
    private void SetStartState()
    {
        if (!activeAtStart)
        {
            ToggleAirstream();
        }
    }
    
    /// <summary>
    /// Adds force to valid objects inside and detaches clyde from gork if necessary
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        if (!other.GetComponent<AirstreamAffected>()) return;

        AddAirstreamForce(other.GetComponent<Rigidbody>());
        var clyde = other.GetComponent<Clyde>();
        if (clyde)
        {
            if (!clyde.state.inAirstream)
            {
                OnClydeAirstreamEntered(clyde);
            }
        }
    }


    /// <summary>
    /// Sets clyde's inAirstream state to false
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        var clyde = other.GetComponent<Clyde>();
        if (clyde)
        {
            clyde.state.inAirstream = false;
        }
    }

    /// <summary>
    /// Applies airstream force to a RigidBody.
    /// </summary>
    /// <param name="rb">RigidBody to apply force to</param>
    private void AddAirstreamForce(Rigidbody rb)
    {
        direction = transform.forward;
        rb.AddForce(direction * strength * Time.deltaTime * 60 * rb.GetComponent<AirstreamAffected>().airstreamForceMultiplier, ForceMode.Acceleration);
    }

    /// <summary>
    /// Used to detach Clyde from gork if he tries to carry Clyde through an airstream.
    /// </summary>
    /// <param name="clyde">Reference to Clyde</param>
    private void OnClydeAirstreamEntered(Clyde clyde)
    {
        clyde.state.inAirstream = true;
        if (!clyde.state.canMove)
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
    /// Toggles particles / collider / fans on/off
    /// </summary>
    public void ToggleAirstream()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc.enabled)
        {
            StopVfx();
            foreach (var ventilator in ventilators)
            {
                StartCoroutine(ventilator.TurnOff());
            }
        }
        else
        {
            PlayVfx();
            foreach (var ventilator in ventilators)
            {
                StartCoroutine(ventilator.TurnOn());
            }
        }
        bc.enabled = !bc.enabled;
    }

    private void PlayVfx()
    {
        foreach (var vfx in  GetComponentsInChildren<ParticleSystem>())
        {
            vfx.Play();
        }
    }
    
    private void StopVfx()
    {
        foreach (var vfx in  GetComponentsInChildren<ParticleSystem>())
        {
            vfx.Stop();
        }
    }
}

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
    private List<AirstreamFan> fans;


    [Header("SFX")]
    
    [SerializeField] 
    private AudioClip activateSFX;
    
    [SerializeField] 
    private AudioClip deactivateSFX;
    
    [SerializeField, Tooltip("Used to play activate / deactivate sounds")] 
    private AudioSource sfxAudioSource;
    
    [SerializeField, Tooltip("Used to play continuous wind sound")] 
    private AudioSource activeAudioSource;
    
    
    
  
    private void Start()
    {
        SetStartState();
    }

    /// <summary>
    /// Toggles airstream / ventilators on/off depending on activeAtStart
    /// </summary>
    private void SetStartState()
    {
        if (!activeAtStart)
        {
            ToggleAirstream();
        }
        foreach (var fan in fans)
        {
            fan.Initialize(activeAtStart);
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
            if (!clyde.inAirstream)
            {
                OnClydeAirstreamEntered(clyde);
            }
        }
    }


    /// <summary>
    /// Sets Clyde's inAirstream state to false
    /// </summary>
    /// <param name="other"></param>
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
        rb.AddForce(direction * strength * Time.deltaTime * 60 * rb.GetComponent<AirstreamAffected>().airstreamForceMultiplier, ForceMode.Acceleration);
    }

    /// <summary>
    /// Used to detach Clyde from Gork if he tries to carry Clyde through an airstream.
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
    /// Toggles particles / collider / fans on / off, plays sfx
    /// </summary>
    public void ToggleAirstream()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc.enabled)
        {
            StopVfx();
            sfxAudioSource.PlayOneShot(deactivateSFX);
            activeAudioSource.Stop();
            foreach (var ventilator in fans)
            {
                ventilator.StopAllCoroutines();
                ventilator.Toggle(false);
            }
        }
        else
        {
            PlayVfx();
            sfxAudioSource.PlayOneShot(activateSFX);
            activeAudioSource.Play();
            foreach (var ventilator in fans)
            {
                ventilator.StopAllCoroutines();
                ventilator.Toggle(true);
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

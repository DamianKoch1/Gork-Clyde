using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class Cable : MonoBehaviour, IActivatable
{

    [SerializeField]
    private Material activeMat, inactiveMat;

    [SerializeField]
    private bool active = false;

    private void Start()
    {
        if (active)
        {
            SetMaterial(activeMat);
        }
        else
        {
            SetMaterial(inactiveMat);
        }
    }

    public void OnButtonActivated()
    {
        ToggleMaterial();
    }

    public void OnButtonDeactivated()
    {
        ToggleMaterial();
    }

    public void OnPlateActivated()
    {
        ToggleMaterial();
    }

    public void OnPlateExited()
    {
        ToggleMaterial();
    }

    /// <summary>
    /// Toggles material to active/inactive version
    /// </summary>
    private void ToggleMaterial()
    {
        if (active)
        {
            SetMaterial(inactiveMat);
        }
        else
        {
            SetMaterial(activeMat);
        }

        active = !active;
    }

    /// <summary>
    /// Sets all children mesh renderers materials to given material
    /// </summary>
    /// <param name="m">new material</param>
    private void SetMaterial(Material m)
    {
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = m;
        }
    }
}

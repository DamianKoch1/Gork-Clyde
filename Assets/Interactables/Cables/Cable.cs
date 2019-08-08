using UnityEngine;

/// <summary>
/// Toggles material of children mesh renderers on (de)activation
/// </summary>
public class Cable : MonoBehaviour, IActivatable
{

    [SerializeField]
    private Material activeMat, inactiveMat;

    [SerializeField]
    private bool activeAtStart = false;

    private void Start()
    {
        SetStartMaterial();
    }

    /// <summary>
    /// Sets starting material depending on activeAtStart
    /// </summary>
    private void SetStartMaterial()
    {
        if (activeAtStart)
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
        if (activeAtStart)
        {
            SetMaterial(inactiveMat);
        }
        else
        {
            SetMaterial(activeMat);
        }

        activeAtStart = !activeAtStart;
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

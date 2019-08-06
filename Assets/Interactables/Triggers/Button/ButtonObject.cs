using UnityEngine;

/// <summary>
/// Button that can be toggled on/off, can (de)activate IActivatable objects
/// </summary>
public class ButtonObject : TriggerObject
{

    [SerializeField]
    private bool oneTimeUse;
    private bool activated;

    /// <summary>
    /// (De)activates all targets, oneTimeUse possible
    /// </summary>
    protected override void OnTriggered()
    {
        if (oneTimeUse)
        {
            if (activated) return;
        }
        if (!activated)
        {
            OnActivated();
        }
        else
        {
            OnDeactivated();
        }
    }

    /// <summary>
    /// Activates targets, changes cable material, plays sound/animation
    /// </summary>
    private void OnActivated()
    {
        activated = true;
        SetCableMaterial(activeMat);
        GetComponent<Animator>().SetBool("BoolTriggerButton", true);
        GetComponent<AudioSource>().Play();
        foreach (GameObject target in targets)
        {
            target.GetComponent<IActivatable>()?.OnButtonActivated();
        }
    }

    /// <summary>
    /// Deactivates targets, changes cable material, plays sound/animation
    /// </summary>
    private void OnDeactivated()
    {
        activated = false;
        SetCableMaterial(inactiveMat);
        GetComponent<Animator>().SetBool("BoolTriggerButton", false);
        GetComponent<AudioSource>().Play();
        foreach (GameObject target in targets)
        {
            target.GetComponent<IActivatable>()?.OnButtonDeactivated();
        }
    }
}

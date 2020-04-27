using UnityEngine;

/// <summary>
/// Button that can be toggled on/off, can (de)activate IActivatable objects
/// </summary>
public class ButtonObject : TriggerObject
{

    [SerializeField]
    private bool oneTimeUse;

    [SerializeField]
    private bool activeAtStart;


    private bool activated;

    private Animator anim;

    private AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetStartState();
    }

    private void SetStartState()
    {
        if (!activeAtStart) return;
        OnActivated();
    }

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
        anim.SetBool("BoolTriggerButton", true);
        audioSource.Play();
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
        anim.SetBool("BoolTriggerButton", false);
        audioSource.Play();
        foreach (GameObject target in targets)
        {
            target.GetComponent<IActivatable>()?.OnButtonDeactivated();
        }
    }
}

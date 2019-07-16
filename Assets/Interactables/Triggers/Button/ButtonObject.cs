using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : TriggerObject
{

    [SerializeField]
    private bool oneTimeUse;
    private bool triggered;

    /// <summary>
    /// (De)activates all targets, oneTimeUse possible
    /// </summary>
    protected override void OnTriggered()
    {
        if (oneTimeUse)
        {
            if (triggered) return;
        }
        if (!triggered)
        {
            triggered = true;
            GetComponent<Animator>().SetBool("BoolTriggerButton", true);
            GetComponent<AudioSource>().Play();
            foreach (GameObject target in targets)
            {
                target.GetComponent<IActivatable>()?.OnButtonActivated();
            }
        }
        else
        {
            triggered = false;
            GetComponent<Animator>().SetBool("BoolTriggerButton", false);
            GetComponent<AudioSource>().Play();
            foreach (GameObject target in targets)
            {
                target.GetComponent<IActivatable>()?.OnButtonDeactivated();
            }
        }
    }
}

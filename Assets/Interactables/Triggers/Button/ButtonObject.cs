using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : TriggerObject
{

    [SerializeField]
    private bool oneTimeUse;
    private bool triggered;


    protected override void OnTriggered()
    {
        if (oneTimeUse)
        {
            if (triggered) return;
        }
        if (!triggered)
        {
            triggered = true;
            GetComponent<AudioSource>().Play();
            foreach (GameObject target in targets)
            {
                target.SendMessage("OnButtonActivated");
            }
        }
        else
        {
            triggered = false;
            GetComponent<AudioSource>().Play();
            foreach (GameObject target in targets)
            {
                target.SendMessage("OnButtonDeactivated");
            }
        }
    }
}

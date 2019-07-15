using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : TriggerObject
{

    private int objectsOnPlateCount;


    private void OnTriggerExit(Collider other)
    {
        if (!MatchesTriggerCondition(other)) return;
        OnPlateExited();
    }

    /// <summary>
    /// Activates targets
    /// </summary>
    protected override void OnTriggered()
    {
        if (objectsOnPlateCount == 0)
        {
            GetComponent<AudioSource>().Play();
            foreach (GameObject target in targets)
            {
                target.GetComponent<IActivatable>()?.OnPlateActivated();
            }
        }
        objectsOnPlateCount++;
    }

    /// <summary>
    /// Deactivates targets
    /// </summary>
    private void OnPlateExited()
    {
        if (objectsOnPlateCount == 1)
        {
            foreach (GameObject target in targets)
            {
                target.GetComponent<IActivatable>()?.OnPlateExited();
            }
        }
        objectsOnPlateCount--;
    }

}

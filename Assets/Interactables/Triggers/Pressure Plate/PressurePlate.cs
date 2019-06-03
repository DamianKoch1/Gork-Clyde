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

    protected override void OnTriggered()
    {
        if (objectsOnPlateCount == 0)
        {
            GetComponent<AudioSource>().Play();
            foreach (GameObject target in targets)
            {
                target.SendMessage("OnPlateActivated");
            }
        }
        objectsOnPlateCount++;
    }

    private void OnPlateExited()
    {
        if (objectsOnPlateCount == 1)
        {
            foreach (GameObject target in targets)
            {
                target.SendMessage("OnPlateExited");
            }
        }
        objectsOnPlateCount--;
    }

}

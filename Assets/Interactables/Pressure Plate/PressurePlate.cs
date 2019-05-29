using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets;

    private int objectsOnPlateCount;

    private enum TriggerableBy {Clyde, Gork, All};
    [SerializeField]
    private TriggerableBy triggerableBy = TriggerableBy.All;
   

    private bool MatchesTriggerCondition(Collider other)
    {
        switch (triggerableBy)
        {
            case TriggerableBy.All:
                if (other.GetComponent<Player>() || other.GetComponent<Pushable>()) return true;
                break;
            
            case TriggerableBy.Gork:
                if (other.GetComponent<Gork>()) return true;
                break;
            
            case TriggerableBy.Clyde:
                if (other.GetComponent<Clyde>()) return true;
                break;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (MatchesTriggerCondition(other))
            {
                PlateEntered();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            if (MatchesTriggerCondition(other))
            {
                PlateExited();
            }
        }
    }


    private void PlateEntered()
    {
        if (objectsOnPlateCount == 0)
        {
            GetComponent<AudioSource>().Play();
            foreach(GameObject target in targets) {
                target.SendMessage("OnPlateActivated");
            }    
        }
        objectsOnPlateCount++;
    }
      
    private void PlateExited()
    {
        if (objectsOnPlateCount == 1)
        {
            foreach(GameObject target in targets)
            {
                target.SendMessage("OnPlateExited");
            }
        }
        objectsOnPlateCount--;
    }
}

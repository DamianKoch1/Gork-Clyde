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
   

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (triggerableBy == TriggerableBy.All)
            {
                if (other.GetComponent<Clyde>() || other.GetComponent<Gork>() || other.CompareTag("pushable"))
                {
                    SendTriggered();
                }
            } else if (triggerableBy == TriggerableBy.Clyde && other.GetComponent<Clyde>())
            {
                SendTriggered();
            } else if (triggerableBy == TriggerableBy.Gork && other.GetComponent<Gork>())
            {
                SendTriggered();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            if (triggerableBy == TriggerableBy.All)
            {
                if (other.GetComponent<Clyde>() || other.GetComponent<Gork>() || other.CompareTag("pushable"))
                {
                    SendExited();
                }
            }
            else if (triggerableBy == TriggerableBy.Clyde && other.GetComponent<Clyde>())
            {
                SendExited();
            }
            else if (triggerableBy == TriggerableBy.Gork && other.GetComponent<Gork>())
            {
                SendExited();
            }
        }
    }

    void SendTriggered()
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
      
    void SendExited()
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

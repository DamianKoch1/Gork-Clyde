using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets;
    [SerializeField]
    private bool oneTimeUse;
    private bool triggered;
    private enum TriggerableBy {Gork, Clide, All};
    [SerializeField]
    private TriggerableBy triggerableBy = TriggerableBy.All;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false)
        {
            if (triggerableBy == TriggerableBy.All)
            {
                if (other.CompareTag("player") || other.CompareTag("pushable"))
                {
                    SendTriggered();
                }
            } else if (triggerableBy == TriggerableBy.Gork && other.GetComponent<Gork>() != null)
            {
                SendTriggered();
            } else if (triggerableBy == TriggerableBy.Clide && other.GetComponent<Clide>() != null)
            {
                SendTriggered();
            }
        }
    }


    void SendTriggered()
    {
        if (oneTimeUse == true)
        {
            if (triggered != true)
            {

                triggered = true;
                foreach(GameObject target in targets) {
                    target.SendMessage("OnButtonActivated");
                }
            }
        }
        else
        {
            foreach (GameObject target in targets)
            {
                target.SendMessage("OnButtonActivated");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
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
        if (!other.isTrigger)
        {
            if (triggerableBy == TriggerableBy.All)
            {
                if (other.CompareTag("player") || other.CompareTag("pushable"))
                {
                    SendTriggered();
                }
            } else if (triggerableBy == TriggerableBy.Gork && other.GetComponent<Gork>())
            {
                SendTriggered();
            } else if (triggerableBy == TriggerableBy.Clide && other.GetComponent<Clyde>())
            {
                SendTriggered();
            }
        }
    }


    void SendTriggered()
    {
        if (oneTimeUse)
        {
            if (!triggered)
            {
                GetComponent<AudioSource>().Play();
                triggered = true;
                foreach(GameObject target in targets) {
                    target.SendMessage("OnButtonActivated");
                }
            }
        }
        else
        {
            GetComponent<AudioSource>().Play();
            foreach (GameObject target in targets)
            {
                target.SendMessage("OnButtonActivated");
            }
        }
    }
}

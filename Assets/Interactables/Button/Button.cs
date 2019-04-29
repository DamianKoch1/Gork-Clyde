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
    private enum TriggerableBy {player1, player2, all};
    [SerializeField]
    private TriggerableBy triggerableBy = TriggerableBy.all;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false)
        {
            if (triggerableBy == TriggerableBy.all)
            {
                if (other.CompareTag("player") || other.CompareTag("pushable"))
                {
                    SendTriggered();
                }
            } else if (triggerableBy == TriggerableBy.player1 && other.CompareTag("Player1"))
            {
                SendTriggered();
            } else if (triggerableBy == TriggerableBy.player2 && other.CompareTag("Player2"))
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

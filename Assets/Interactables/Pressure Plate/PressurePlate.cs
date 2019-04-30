using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets;

    private enum TriggerableBy {player1, player2, all};
    [SerializeField]
    private TriggerableBy triggerableBy = TriggerableBy.all;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false)
        {
            if (triggerableBy == TriggerableBy.all)
            {
                if (other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("pushable"))
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
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger == false)
        {
            if (triggerableBy == TriggerableBy.all)
            {
                if (other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("pushable"))
                {
                    SendExited();
                }
            }
            else if (triggerableBy == TriggerableBy.player1 && other.CompareTag("Player1"))
            {
                SendExited();
            }
            else if (triggerableBy == TriggerableBy.player2 && other.CompareTag("Player2"))
            {
                SendExited();
            }
        }
    }

    void SendTriggered()
    {
        foreach(GameObject target in targets) {
            target.SendMessage("OnPlateActivated");
        }
    }
      
    void SendExited()
    {
        foreach(GameObject target in targets)
        {
            target.SendMessage("OnPlateExited");
        }
    }
}

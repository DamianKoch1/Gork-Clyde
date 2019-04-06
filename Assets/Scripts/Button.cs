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
    public enum TriggerableBy {player1, player2, all};
    public TriggerableBy triggerableBy = TriggerableBy.all;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (triggerableBy == TriggerableBy.all)
        {
            if (other.CompareTag("Player1") || other.CompareTag("Player2"))
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
    private void OnTriggerExit(Collider other)
    {
        if (triggerableBy == TriggerableBy.all)
        {
            if (other.CompareTag("Player1") || other.CompareTag("Player2"))
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
    void SendExited()
    {
        foreach(GameObject target in targets)
        {
            target.SendMessage("OnButtonExited");
        }
    }
}

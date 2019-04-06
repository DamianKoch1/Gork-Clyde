using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
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
                target.SendMessage("OnButtonActivated");
            }
        } else if (triggerableBy == TriggerableBy.player1 && other.CompareTag("Player1"))
        {
            target.SendMessage("OnButtonActivated");
        } else if (triggerableBy == TriggerableBy.player2 && other.CompareTag("Player2"))
        {
            target.SendMessage("OnButtonActivated");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (triggerableBy == TriggerableBy.all)
        {
            if (other.CompareTag("Player1") || other.CompareTag("Player2"))
            {
                target.SendMessage("OnButtonExited");
            }
        }
        else if (triggerableBy == TriggerableBy.player1 && other.CompareTag("Player1"))
        {
            target.SendMessage("OnButtonExited");
        }
        else if (triggerableBy == TriggerableBy.player2 && other.CompareTag("Player2"))
        {
            target.SendMessage("OnButtonExited");
        }
    }
}

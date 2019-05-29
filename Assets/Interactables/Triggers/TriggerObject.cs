using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerObject : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] targets;

    private enum TriggerableBy {Clyde, Gork, All};
    [SerializeField]
    private TriggerableBy triggerableBy = TriggerableBy.All;
   

    private void OnTriggerEnter(Collider other)
    {
        if (MatchesTriggerCondition(other))
        {
            OnTriggered();
        }
    }
    
    protected bool MatchesTriggerCondition(Collider other)
    {
        if (other.isTrigger) return false;
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
  
    protected abstract void OnTriggered();
   
      
   
}

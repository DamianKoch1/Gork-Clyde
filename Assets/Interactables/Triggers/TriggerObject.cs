using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerObject : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] targets;

    [SerializeField]
    protected Renderer cableRenderer;

    [SerializeField]
    protected Material activeMat, inactiveMat;

    private enum TriggerableBy {Clyde, Gork, All};
    [SerializeField]
    private TriggerableBy triggerableBy = TriggerableBy.All;
   

    private void OnTriggerEnter(Collider other)
    {
        if (!MatchesTriggerCondition(other)) return;
        OnTriggered();
    }
    
    /// <summary>
    /// Checks if collider is able to trigger this object
    /// </summary>
    /// <param name="other">collider to check</param>
    /// <returns></returns>
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
  

    /// <summary>
    /// Sets own cable mesh renderer material to given material
    /// </summary>
    /// <param name="m">new material</param>
    protected void SetCableMaterial(Material m)
    {
        cableRenderer.material = m;
    }
   
}

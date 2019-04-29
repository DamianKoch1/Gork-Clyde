using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObjectOnTop : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false)
        {
            other.transform.SetParent(target, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == target)
        {
            other.transform.SetParent(null, true);
        }
    }
}

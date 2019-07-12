using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockableAirstream : MonoBehaviour
{
    [SerializeField]
    private Airstream blockableAirstream;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("platform"))
        {
            blockableAirstream.ToggleAirstream();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("platform"))
        {
            blockableAirstream.ToggleAirstream();
        }
    }
}

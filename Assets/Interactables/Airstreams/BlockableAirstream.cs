using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockableAirstream : MonoBehaviour
{
    [SerializeField]
    private GameObject blockableAirstream;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("platform"))
        {
            blockableAirstream.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("platform"))
        {
            blockableAirstream.SetActive(true);
        }
    }
}

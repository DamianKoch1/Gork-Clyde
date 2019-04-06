using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawPlatform : MonoBehaviour
{
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
        if (other.CompareTag("IgnorePlatform") == false)
        {
            other.transform.SetParent(transform, true);
        }
        if (other.CompareTag("Player1"))
        {
            transform.parent.SendMessage("OnPlatformTriggered", gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform && other.CompareTag("IgnorePlatform") == false)
        {
            other.transform.SetParent(null, true);
        }
    }
}

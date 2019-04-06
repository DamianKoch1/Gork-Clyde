using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstream : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField]
    private float strength;
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            object[] temp = new object[2];
            temp[0] = direction;
            temp[1] = strength;
            other.SendMessage("ToggleAirstream", temp);
            Debug.LogWarning("entered");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            other.SendMessage("ToggleAirstream", new object[2]);
            Debug.LogWarning("exited");
        }
    }
}

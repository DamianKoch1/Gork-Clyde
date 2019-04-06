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
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            object[] temp = new object[3];
            temp[0] = true;
            temp[1] = direction;
            temp[2] = strength;
            other.SendMessage("ToggleAirstream", temp);
        }
        

    }
    private void OnTriggerExit(Collider other)
    {
      
        if (other.CompareTag("Player2"))
        {
            object[] temp = new object[3];
            temp[0] = false;
            temp[1] = 0;
            temp[2] = 0;
            other.SendMessage("ToggleAirstream", temp);
        }
        
    }

    public void OnButtonActivated()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        BoxCollider bc = GetComponent<BoxCollider>();
        mr.enabled = !mr.enabled;
        bc.enabled = !bc.enabled;
    }
}

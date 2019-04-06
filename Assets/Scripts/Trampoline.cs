using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    private float bounceAmplifier, maxBounceStrength;
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
        if (other.CompareTag("Player2"))
        {
            object[] temp = new object[2];
            temp[0] = bounceAmplifier;
            temp[1] = maxBounceStrength;
            other.SendMessage("Bounce", temp);
        }

    }
}

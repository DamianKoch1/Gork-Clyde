using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    private float bounceAmplifier, maxBounceStrength, threshholdVelocity = 10f;
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
            object[] temp = new object[3];
            temp[0] = bounceAmplifier;
            temp[1] = maxBounceStrength;
            temp[2] = threshholdVelocity;
            other.SendMessage("Bounce", temp);
        }

    }
}

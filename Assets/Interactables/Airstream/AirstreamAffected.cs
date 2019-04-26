using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstreamAffected : MonoBehaviour
{
    [HideInInspector]
    public Vector3 airstreamMotion;
    [HideInInspector]
    public bool inAirstream = false;

    private void FixedUpdate()
    {
        if (inAirstream == false)
        {
            airstreamMotion *= 0.95f;
        }
    }
}

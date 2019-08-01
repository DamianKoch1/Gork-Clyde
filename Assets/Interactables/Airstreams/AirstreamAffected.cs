using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirstreamAffected : MonoBehaviour
{
    [Range(0, 10)]
    public float airstreamForceMultiplier = 1;

}

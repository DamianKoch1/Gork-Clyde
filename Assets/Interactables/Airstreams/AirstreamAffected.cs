using UnityEngine;

/// <summary>
/// Add to any gameObject to make it affected by airstreams
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AirstreamAffected : MonoBehaviour
{
    [Range(0, 10)]
    public float airstreamForceMultiplier = 1;

}

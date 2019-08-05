using UnityEngine;

/// <summary>
/// Add to any gameObject to make it carryable by Gork
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Carryable : MonoBehaviour
{
    [HideInInspector] 
    public bool isHeld = false;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to any object to make it cast a shadow on the ground
/// </summary>
public class PlayerShadow : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.parent.position, -Vector3.up, out var raycastHit, Mathf.Infinity, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
        transform.position = raycastHit.point + Vector3.up*0.1f;

    }
}

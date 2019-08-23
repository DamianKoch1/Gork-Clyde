using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to any object to make it cast a shadow on the ground
/// </summary>
public class PlayerShadow : MonoBehaviour
{
    void Update()
    {
        UpdateShadowPosition();
    }

    /// <summary>
    /// Raycasts downwards from parent and sets shadow slightly above hit point
    /// </summary>
    private void UpdateShadowPosition()
    {
        Physics.Raycast(transform.parent.position, -Vector3.up, out var shadowPos, Mathf.Infinity, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
        transform.position = shadowPos.point + Vector3.up * 0.1f;
    }
}

using System.Collections;
using UnityEngine;

/// <summary>
/// Add to any gameObject to make it respawn when entering DeathZone
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Respawning : MonoBehaviour
{

    private Vector3 spawnpoint;
    private Quaternion rotation;
    private Rigidbody rb;

    [SerializeField]
    private bool dynamicSpawnpoint = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(UpdateSpawnPoint());
    }

    
    /// <summary>
    /// Respawn at last saved spawnpoint, resets rotation/velocity
    /// </summary>
    public void Respawn()
    {
        GetComponent<Player>()?.ResetMotion();
        rb.MovePosition(spawnpoint);
        transform.rotation = rotation;
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// If dynamicSpawnpoint is false, only sets spawnpoint at start
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateSpawnPoint()
    {
        yield return null;
        SetSpawnPoint();
        while (dynamicSpawnpoint)
        {
            if (Physics.Raycast(rb.position, -Vector3.up,
            out var raycastHit, 2f,Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (IsValidSpawnpoint(raycastHit))
                {
                    SetSpawnPoint();
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Checks if a RaycastHit contains a valid spawnpoint
    /// </summary>
    /// <param name="hit">RaycastHit to check</param>
    /// <returns></returns>
    private bool IsValidSpawnpoint(RaycastHit hit)
    {
        if (hit.transform.CompareTag("platform")) return false;
        if (hit.transform.CompareTag("noSpawnpoint")) return false;
        if (GetComponent<Carryable>()?.isHeld == true) return false;
        if (GetComponent<Clyde>()?.inAirstream == true) return false;
        return true;
    }
        
    /// <summary>
    /// Saves spawnpoint/rotation
    /// </summary>
    private void SetSpawnPoint()
    {
        spawnpoint = rb.position;
        rotation = transform.rotation;
    }
    
}

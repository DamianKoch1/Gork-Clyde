using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Respawning : MonoBehaviour
{

    private Vector3 spawnpoint;
    private Rigidbody rb;

    [SerializeField]
    private bool dynamicSpawnpoint = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckSpawnPoint());
    }

    
    /// <summary>
    /// Respawn at last saved spawnpoint
    /// </summary>
    public void Respawn()
    {
        GetComponent<Player>()?.ResetMotion();
        rb.velocity = Vector3.zero;
        rb.MovePosition(spawnpoint);
    }

    /// <summary>
    /// If dynamic spawnpoint is true, keeps updating spawnpoint if at valid position
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckSpawnPoint()
    {
        SetSpawnPoint();
        while (dynamicSpawnpoint)
        {
            if (Physics.Raycast(rb.position, -Vector3.up,
            2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                SetSpawnPoint();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    
    
    private void SetSpawnPoint()
    {
        spawnpoint = rb.position;
    }
    
}

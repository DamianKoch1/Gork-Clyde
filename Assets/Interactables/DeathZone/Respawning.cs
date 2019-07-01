using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Respawning : MonoBehaviour
{

    private Vector3 spawnpoint;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckSpawnPoint());
    }

    
    public void Respawn()
    {
        if (GetComponent<Player>())
        {
            GetComponent<Player>().ResetMotion();
        }
        rb.velocity = Vector3.zero;
        rb.MovePosition(spawnpoint);
    }

    public IEnumerator CheckSpawnPoint()
    {
        SetSpawnPoint();
        while (true)
        {
            //prevent spawning too close to edge
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

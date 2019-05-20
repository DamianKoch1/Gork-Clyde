using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField]
    public static GameObject SPAWNPOINT;

    private void Start()
    {
        SPAWNPOINT = gameObject;
    }
}

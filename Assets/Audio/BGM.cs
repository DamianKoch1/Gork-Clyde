using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM instance;

    public static BGM Instance = this;
  


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (ExistsAlready())
        {
            Destroy(gameObject);
        }
    }

    private bool ExistsAlready()
    {
        return (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1);
    }

}

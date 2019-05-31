using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAudioLoaded : MonoBehaviour
{
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
        if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1) return true;
        return false;
    }

}

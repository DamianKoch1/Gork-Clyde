using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAudioLoaded : MonoBehaviour
{
    // Start is called before the first frame update
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
        if (CompareTag("bgm"))
        {
            if (GameObject.FindGameObjectsWithTag("bgm").Length <= 1) return false;
        }
        if (CompareTag("sfx"))
        {
            if (GameObject.FindGameObjectsWithTag("sfx").Length <= 1) return false;
        }
        return false;
    }
    
}

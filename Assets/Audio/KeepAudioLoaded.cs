using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAudioLoaded : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("bgm").Length > 1 && CompareTag("bgm"))
        {
            Destroy(gameObject);
        }
        if (GameObject.FindGameObjectsWithTag("sfx").Length > 1 && CompareTag("sfx"))
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

}

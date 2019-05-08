using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBgmLoaded : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("bgm").Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

}

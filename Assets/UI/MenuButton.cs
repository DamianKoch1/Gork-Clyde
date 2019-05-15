using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip clickSound;
    
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>().clip = clickSound;
        GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>().Play();
    }
}

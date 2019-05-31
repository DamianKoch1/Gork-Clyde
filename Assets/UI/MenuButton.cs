using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip clickSound;

    private EventSystem evtSystem;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
        evtSystem = EventSystem.current;
    }

    private void PlayClickSound()
    {
        GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>().clip = clickSound;
        GameObject.FindGameObjectWithTag("sfx").GetComponent<AudioSource>().Play();
    }

    public void GrabFocus()
    {
        evtSystem.SetSelectedGameObject(gameObject);
    }
    
}

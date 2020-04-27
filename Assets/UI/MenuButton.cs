using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MenuButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void GrabFocus()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    /// <summary>
    /// Prevents triggering invisible buttons from hidden popups
    /// </summary>
    public static void RemoveFocus()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

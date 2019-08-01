﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
        SFX.Instance.PlaySound(clickSound);
    }

    public void GrabFocus()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    /// <summary>
    /// Prevents triggering invisible buttons from hidden popups
    /// </summary>
    public static void FocusNothing()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

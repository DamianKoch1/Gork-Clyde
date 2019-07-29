using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{

    [SerializeField]
    private Button gorkButton, clydeButton, playButton;

    private bool gorkSelected = true;
    private EventSystem evtSystem;

    /// <summary>
    /// story panel that play button will load
    /// </summary>
    public static string StoryPanelName;

    
    
    void Start()
    {
        InitializeButtons();
    }

    //disable gork/clyde/play button based on previous selection
    private void InitializeButtons()
    {
        evtSystem = EventSystem.current;
        gorkButton.onClick.AddListener(GorkSelected);
        clydeButton.onClick.AddListener(ClydeSelected);
        if (String.IsNullOrEmpty(Gork.XAxis))
        {
            playButton.interactable = false;
        }
        else if (gorkSelected)
        {
            gorkButton.interactable = false;
        }
        else
        {
            clydeButton.interactable = false;
        }
    }

    /// <summary>
    /// Setup input axes accordingly, toggle gork/clyde button enabled
    /// </summary>
    void GorkSelected()
    {
        gorkSelected = true;
        gorkButton.interactable = false;
        clydeButton.interactable = true;
        playButton.interactable = true;
        Gork.XAxis = "GorkHorizontal";
        Gork.ZAxis = "GorkVertical";
        Gork.JumpButton = "GorkJump";
        Gork.GorkInteract = "GorkInteract";
        Gork.GorkCam = "GorkCam";
        Clyde.XAxis = "ClydeHorizontal";
        Clyde.ZAxis = "ClydeVertical";
        Clyde.JumpButton = "ClydeJump";
        Clyde.ClydeInteract = "ClydeInteract";
        Clyde.ClydeCam = "ClydeCam";
        evtSystem.SetSelectedGameObject(playButton.gameObject);
    }

    /// <summary>
    /// Setup input axes accordingly, toggle gork/clyde button enabled
    /// </summary>
    void ClydeSelected()
    {
        gorkSelected = false;
        clydeButton.interactable = false;
        gorkButton.interactable = true;
        playButton.interactable = true;
        Gork.XAxis = "ClydeHorizontal";
        Gork.ZAxis = "ClydeVertical";
        Gork.JumpButton = "ClydeJump";
        Gork.GorkInteract = "ClydeInteract";
        Gork.GorkCam = "ClydeCam";
        Clyde.XAxis = "GorkHorizontal";
        Clyde.ZAxis = "GorkVertical";
        Clyde.JumpButton = "GorkJump";
        Clyde.ClydeInteract = "GorkInteract";
        Clyde.ClydeCam = "GorkCam";
        evtSystem.SetSelectedGameObject(playButton.gameObject);
    }

    public void LoadStoryPanel()
    {
        SceneManager.LoadScene(StoryPanelName);
    }


}

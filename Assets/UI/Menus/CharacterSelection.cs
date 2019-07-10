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
        Clyde.XAxis = "ClydeHorizontal";
        Clyde.ZAxis = "ClydeVertical";
        Clyde.JumpButton = "ClydeJump";
        evtSystem.SetSelectedGameObject(playButton.gameObject);
    }

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
        Clyde.XAxis = "GorkHorizontal";
        Clyde.ZAxis = "GorkVertical";
        Clyde.JumpButton = "GorkJump";
        evtSystem.SetSelectedGameObject(playButton.gameObject);
    }

    public void LoadStoryPanel()
    {
        SceneManager.LoadScene(StoryPanelName);
    }


}

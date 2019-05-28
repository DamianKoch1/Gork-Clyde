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

    public static string NEXT_LEVEL_NAME;
    
    // Start is called before the first frame update
    void Start()
    {
        evtSystem = EventSystem.current;
        gorkButton.onClick.AddListener(GorkSelected);
        clydeButton.onClick.AddListener(ClydeSelected);
        if (String.IsNullOrEmpty(Gork.XAXIS))
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
        Gork.XAXIS = "GorkHorizontal";
        Gork.ZAXIS = "GorkVertical";
        Gork.JUMPBUTTON = "GorkJump";
        Gork.GORKINTERACT = "GorkInteract";
        Clyde.XAXIS = "ClydeHorizontal";
        Clyde.ZAXIS = "ClydeVertical";
        Clyde.JUMPBUTTON = "ClydeJump";
        evtSystem.SetSelectedGameObject(clydeButton.gameObject);
    }

    void ClydeSelected()
    {
        gorkSelected = false;
        clydeButton.interactable = false;
        gorkButton.interactable = true;
        playButton.interactable = true;
        Gork.XAXIS = "ClydeHorizontal";
        Gork.ZAXIS = "ClydeVertical";
        Gork.JUMPBUTTON = "ClydeJump";
        Gork.GORKINTERACT = "ClydeInteract";
        Clyde.XAXIS = "GorkHorizontal";
        Clyde.ZAXIS = "GorkVertical";
        Clyde.JUMPBUTTON = "GorkJump";
        evtSystem.SetSelectedGameObject(gorkButton.gameObject);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(NEXT_LEVEL_NAME);
    }
    
    
}

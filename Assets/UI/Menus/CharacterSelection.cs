using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{

    [SerializeField] 
    private Button gorkButton, clydeButton, playButton;

    private static bool gorkSelected = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gorkButton.onClick.AddListener(GorkSelected);
        clydeButton.onClick.AddListener(ClydeSelected);
        if (String.IsNullOrEmpty(Gork._xAxis))
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
        Gork._xAxis = "GorkHorizontal";
        Gork._zAxis = "GorkVertical";
        Gork._jumpButton = "GorkJump";
        Gork._gorkInteract = "GorkInteract";
        Clyde._xAxis = "ClydeHorizontal";
        Clyde._zAxis = "ClydeVertical";
        Clyde._jumpButton = "ClydeJump";
    }

    void ClydeSelected()
    {
        gorkSelected = false;
        clydeButton.interactable = false;
        gorkButton.interactable = true;
        playButton.interactable = true;
        Gork._xAxis = "ClydeHorizontal";
        Gork._zAxis = "ClydeVertical";
        Gork._jumpButton = "ClydeJump";
        Gork._gorkInteract = "ClydeInteract";
        Clyde._xAxis = "GorkHorizontal";
        Clyde._zAxis = "GorkVertical";
        Clyde._jumpButton = "GorkJump";
    }
    
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Player
{
    public static string XAXIS = "ClydeHorizontal", ZAXIS = "ClydeVertical", JUMPBUTTON = "ClydeJump";

    private void Start()
    {
        base.Start();
        //for playtesting (if somehow character selection is skipped)
        if (String.IsNullOrEmpty(XAXIS))
        {
            XAXIS = "ClydeHorizontal";
            ZAXIS = "ClydeVertical";
            JUMPBUTTON = "ClydeJump";
        }
        //        
        InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
    }
}
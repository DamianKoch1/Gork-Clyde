using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Player
{
    public static string _xAxis = "ClydeHorizontal", _zAxis = "ClydeVertical", _jumpButton = "ClydeJump";

    private void Start()
    {
        base.Start();
        //for playtesting (if somehow character selection is skipped)
        if (String.IsNullOrEmpty(_xAxis))
        {
            _xAxis = "ClydeHorizontal";
            _zAxis = "ClydeVertical";
            _jumpButton = "ClydeJump";
        }
        //        
        InitializeInputs(_xAxis, _zAxis, _jumpButton);
    }
}
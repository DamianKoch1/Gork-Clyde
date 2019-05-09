using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Player
{
    public static string _xAxis, _zAxis, _jumpButton;

    private void Start()
    {
        base.Start();
        InitializeInputs(_xAxis, _zAxis, _jumpButton);
    }
}
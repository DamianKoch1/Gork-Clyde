using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Player
{
   

    protected override void InitializeInputs()
    {
        xAxis = "ClideHorizontal";
        zAxis = "ClideVertical";
        jumpButton = "ClideJump";
    }



    
}
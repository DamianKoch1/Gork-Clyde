using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clide : Player
{
   

    protected override void InitializeInputs()
    {
        xAxis = "ClideHorizontal";
        zAxis = "ClideVertical";
        jumpButton = "ClideJump";
    }



    
}
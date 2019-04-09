using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gork : Player
{
    protected override void InitializeInputs()
    {
        xAxis = "Horizontal";
        zAxis = "Vertical";
        jumpButton = "Jump";
    }
}

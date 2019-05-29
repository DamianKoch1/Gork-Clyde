using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPushable : Pushable
{
    private int touchedPlayerCount = 0;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Player>())
        {
            touchedPlayerCount++;
            isPushed = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.GetComponent<Player>())
        {
            touchedPlayerCount--;
            if (touchedPlayerCount == 0)
            {
                isPushed = false;
            }
        }
    }

}

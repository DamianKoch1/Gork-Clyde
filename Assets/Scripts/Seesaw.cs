using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    private GameObject platform1, platform2, pushedPlatform;
    private float pos1Y, pos2Y;
    void Start()
    {
        platform1 = transform.Find("Platform1").gameObject;
        platform2 = transform.Find("Platform2").gameObject;
        pos1Y = transform.Find("Pos1").transform.position.y;
        pos2Y = transform.Find("Pos2").transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (platform1.transform.position.y != platform2.transform.position.y)
        {
           //send msg from platform containing platform triggered + trigger object + trigger object offset, set triggered platform to object + offset position, other platform to *-1, clamp between pos1/2
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    private GameObject platform1, platform2, pushedPlatform;
    private float minHeight, lerpAmount = 0;
    private Vector3 startpos, targetpos;
    void Start()
    {
        platform1 = transform.Find("Platform1").gameObject;
        minHeight = transform.Find("MinHeight").transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pushedPlatform != null)
        {
            lerpAmount += Time.deltaTime;
            pushedPlatform.transform.position = Vector3.Lerp(startpos, targetpos, lerpAmount);
            platform2.transform.localPosition = new Vector3(platform2.transform.localPosition.x, -pushedPlatform.transform.localPosition.y, platform2.transform.localPosition.z);
            if (lerpAmount >= 1)
            {
                lerpAmount = 0;
                pushedPlatform = null;
                platform2 = null;
            }
        }
    }

    public void OnPlatformTriggered(GameObject triggeredPlatform)
    {
        if (triggeredPlatform.transform.position.y > minHeight && lerpAmount == 0)
        {
            pushedPlatform = triggeredPlatform;
            startpos = pushedPlatform.transform.position;
            if (pushedPlatform == platform1)
            {
                platform2 = transform.Find("Platform2").gameObject;
                targetpos = pushedPlatform.transform.position;
                targetpos.y = minHeight;
            }
            else
            {
                platform2 = transform.Find("Platform1").gameObject;
                targetpos = pushedPlatform.transform.position;
                targetpos.y = minHeight;
            }
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicators : MonoBehaviour
{
    [SerializeField]
    private Transform gorkTransform, clydeTransform;
    [SerializeField]
    private RectTransform canvasRect;
    private Vector3 gorkPos, clydePos;
    private Camera cam;
    [SerializeField]
    private GameObject gorkIndicator, clydeIndicator;

    [Range(0, 0.1f)]
    public float indicatorDistanceX = 0.05f, indicatorDistanceY = 0.05f;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        CheckGork();
        CheckClyde();
    }

    
    /// <summary>
    /// Updates gork indicator if gork is out of screen
    /// </summary>
    private void CheckGork()
    {
        gorkPos = cam.WorldToScreenPoint(gorkTransform.position);
        if (!cam.pixelRect.Contains(gorkPos))
        {
            UpdateIndicator(gorkIndicator, gorkPos);
        }
        else if (gorkIndicator.activeSelf)
        {
            gorkIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Updates clyde indicator if clyde is out of screen
    /// </summary>
    private void CheckClyde()
    {
        clydePos = cam.WorldToScreenPoint(clydeTransform.position);
        if (!cam.pixelRect.Contains(clydePos))
        {
            UpdateIndicator(clydeIndicator, clydePos);
        }
        else if (clydeIndicator.activeSelf)
        {
            clydeIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Move Indicator to screen border pointing to player screen position
    /// </summary>
    /// <param name="indicator"></param>
    /// <param name="targetPos"></param>
    private void UpdateIndicator(GameObject indicator, Vector3 targetPos)
    {
        Vector2 rectSize = canvasRect.sizeDelta;

        RectTransform indicatorRect = indicator.GetComponent<RectTransform>();

        if (!indicator.activeSelf)
        {
            indicator.SetActive(true);
        }

        Vector2 indicatorTargetPos = cam.ScreenToViewportPoint(targetPos);

        //rotating indicator based on screen edge position
        Vector2 rotationMultiplier = indicatorTargetPos;
        rotationMultiplier.x = Mathf.Clamp(rotationMultiplier.x, 0, 1);
        rotationMultiplier.y = Mathf.Clamp(rotationMultiplier.y, 0, 1);
        float zRotationAngles = 0;

        if (rotationMultiplier.x == 0 || rotationMultiplier.y == 1)
        {
            zRotationAngles = -45 - rotationMultiplier.x * 90 - rotationMultiplier.y * 90;
        }
        else
        {
            zRotationAngles = -45 + rotationMultiplier.x * 90 + rotationMultiplier.y * 90;
        }
       

        indicatorRect.eulerAngles = new Vector3(0, 0, zRotationAngles);

        //applying min distance from screen edge
        indicatorTargetPos.x = Mathf.Clamp(indicatorTargetPos.x, indicatorDistanceX, 1 - indicatorDistanceX);
        indicatorTargetPos.y = Mathf.Clamp(indicatorTargetPos.y, indicatorDistanceY, 1 - indicatorDistanceY);
       
        indicatorTargetPos.x *= rectSize.x;
        indicatorTargetPos.y *= rectSize.y;
        indicatorRect.anchoredPosition = indicatorTargetPos;
    }

}

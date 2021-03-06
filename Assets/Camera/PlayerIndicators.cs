﻿using UnityEngine;

public class PlayerIndicators : MonoBehaviour
{
    [SerializeField]
    private Transform gorkTransform;

    [SerializeField]
    private Transform clydeTransform;

    [SerializeField]
    private RectTransform canvasRect;

    private Camera cam;

    [SerializeField]
    private GameObject gorkIndicator;

    [SerializeField]
    private GameObject clydeIndicator;

    [SerializeField]
    private GameObject gorkBg;

    [SerializeField]
    private GameObject clydeBg;

    [Range(0, 0.1f)]
    public float indicatorDistanceX = 0.05f;

    [Range(0, 0.1f)]
    public float indicatorDistanceY = 0.05f;

    private void Start()
    {
        cam = GetComponent<Camera>();
        gorkIndicator.SetActive(true);
        clydeIndicator.SetActive(true);
    }

    private void Update()
    {
        UpdateIndicator(gorkTransform, gorkIndicator, gorkBg);
        UpdateIndicator(clydeTransform, clydeIndicator, clydeBg);
    }

    /// <summary>
    /// Updates player indicator if it is out of screen
    /// </summary>
    private void UpdateIndicator(Transform playerTransform, GameObject indicator, GameObject bg)
    {
        var playerScreenPos = cam.WorldToScreenPoint(playerTransform.position);
        if (!cam.pixelRect.Contains(playerScreenPos))
        {
            MoveIndicator(indicator, bg, playerScreenPos);
        }
        else if (indicator.activeSelf)
        {
            indicator.SetActive(false);
            bg.SetActive(false);
        }
    }


    /// <summary>
    /// Move Indicator to screen border pointing to player screen position
    /// </summary>
    /// <param name="indicator">indicator to update</param>
    /// <param name="bg">indicator background to update</param>
    /// <param name="targetPos">position of indicator target</param>
    private void MoveIndicator(GameObject indicator, GameObject bg, Vector3 targetPos)
    {
        Vector2 rectSize = canvasRect.sizeDelta;

        RectTransform indicatorRect = indicator.GetComponent<RectTransform>();
        RectTransform bgRect = bg.GetComponent<RectTransform>();

        if (!indicator.activeSelf)
        {
            indicator.SetActive(true);
        }

        if (!bg.activeSelf)
        {
            bg.SetActive(true);
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

        bgRect.eulerAngles = new Vector3(0, 0, zRotationAngles);

        //applying min distance from screen edge
        indicatorTargetPos.x = Mathf.Clamp(indicatorTargetPos.x, indicatorDistanceX, 1 - indicatorDistanceX);
        indicatorTargetPos.y = Mathf.Clamp(indicatorTargetPos.y, indicatorDistanceY, 1 - indicatorDistanceY);

        indicatorTargetPos.x *= rectSize.x;
        indicatorTargetPos.y *= rectSize.y;

        indicatorRect.anchoredPosition = indicatorTargetPos;
        bgRect.anchoredPosition = indicatorTargetPos;
    }

}

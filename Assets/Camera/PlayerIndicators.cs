using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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

    private void UpdateIndicator(GameObject indicator, Vector3 target)
    {
        if (!indicator.activeSelf)
        {
            indicator.SetActive(true);
        }
        Vector3 indicatorPos = cam.ScreenToViewportPoint(target);
        indicatorPos.x = Mathf.Clamp(indicatorPos.x, indicatorDistanceX, 1-indicatorDistanceX);
        indicatorPos.y = Mathf.Clamp(indicatorPos.y, indicatorDistanceY, 1-indicatorDistanceY);
        indicatorPos.x *= canvasRect.sizeDelta.x;
        indicatorPos.y *= canvasRect.sizeDelta.y;
        indicator.GetComponent<RectTransform>().anchoredPosition = indicatorPos;
       
    }
    
    
}

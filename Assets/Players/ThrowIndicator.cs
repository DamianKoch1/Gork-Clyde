using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowIndicator : MonoBehaviour
{
    [SerializeField] 
    [Range(10, 50)] 
    private int throwIndicatorCount = 20;

    [SerializeField] 
    [Range(0.06f, 0.1f)] 
    private float throwIndicatorDetail = 0.08f;
    
    [SerializeField]
    private GameObject landingIndicatorPrefab;
    private GameObject landingIndicator;

    private Vector3 throwVector;
    private GameObject heldObject;
    
    [SerializeField]
    private LineRenderer lineRenderer;

    private RaycastHit hit;
    
    public void UpdateIndicator(Vector3 _throwVector, GameObject _heldObject)
    {
        throwVector = _throwVector;
        heldObject = _heldObject;
        
        lineRenderer.positionCount = throwIndicatorCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            var pointPosition = PointPosAtTime(i * throwIndicatorDetail);
            if (HitGround(i))
            {
                DestroyIndicator(i+1, false);
                lineRenderer.SetPosition(i, hit.point);
                if (!landingIndicator)
                {
                    landingIndicator = Instantiate(landingIndicatorPrefab);
                }
                landingIndicator.transform.up = hit.normal;
                landingIndicator.transform.position = hit.point + landingIndicator.transform.up * 0.1f;
                return;
            }
            lineRenderer.SetPosition(i, pointPosition);
        }
        DestroyLandingIndicator();
    }

    private bool HitGround(int atIndex)
    {
        var pointPosition = PointPosAtTime(atIndex * throwIndicatorDetail);
        var nextPointPosition = PointPosAtTime((atIndex + 1) * throwIndicatorDetail);
        var distanceToNext = Vector3.Distance(pointPosition, nextPointPosition);
        if (Physics.Raycast(pointPosition, nextPointPosition - pointPosition, out hit, distanceToNext, Physics.AllLayers,
        QueryTriggerInteraction.Ignore)) return true;
        return false;
    }
    
    public void DestroyIndicator(int from = 0, bool destroyLandingIndicator = true)
    {
        lineRenderer.positionCount = from;
        if (destroyLandingIndicator)
        {
            DestroyLandingIndicator();
        }
    }

    private void DestroyLandingIndicator()
    {
        if (landingIndicator)
        {
            Destroy(landingIndicator);
        }
    }
    
    private Vector3 PointPosAtTime(float time) 
    {
        float amplifier = 1;
        
        if (!heldObject.GetComponent<Clyde>())
        {
            amplifier = 0.5f;
        }
        return lineRenderer.gameObject.transform.position + throwVector * time + Physics.gravity * time * time * amplifier;
    }
}    


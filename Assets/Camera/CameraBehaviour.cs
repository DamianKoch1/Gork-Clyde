using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject gork, clide;
    private Vector3 offset;
    private Vector3 playerMiddle;
    [SerializeField]
    private float playerDistanceZoomThreshhold;
    private float zoomMultiplier;
    [SerializeField]
    private float minZoom, maxZoom, zoomSpeed, followSpeed, rotateSpeed = 2;
    private Vector3 desiredPos, targetPos;
    private RaycastHit hit;
    [SerializeField]
    private LayerMask wallLayers;
    [SerializeField] 
    private GameObject pauseMenu, optionsMenu;
    void Start()
    {
        if (gork != null && clide != null)
        {
           // playerMiddle = gork.transform.position + (0.5f * (clide.transform.position - gork.transform.position));
            playerMiddle = 0.5f * (gork.transform.position + clide.transform.position);
            offset = transform.position - playerMiddle;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (pauseMenu.activeSelf)
            {
                if (optionsMenu.activeSelf)
                {
                    optionsMenu.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                }
            }
            else
            {
                pauseMenu.SetActive( true);
                Time.timeScale = 0f;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X")*rotateSpeed, Vector3.up) * offset;
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X")*rotateSpeed);
        }
        if (Input.GetAxis("Mouse Y") != 0)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotateSpeed, -transform.right) * offset;
            transform.RotateAround(transform.position, -transform.right, Input.GetAxis("Mouse Y") * rotateSpeed);
        }
        if (gork != null && clide != null)
        {
            playerMiddle = gork.transform.position + (0.5f * (clide.transform.position - gork.transform.position));
            zoomMultiplier = Mathf.Clamp((playerDistanceZoomThreshhold / (Vector3.Distance(gork.transform.position, clide.transform.position)))/zoomSpeed, minZoom, maxZoom);
            desiredPos = playerMiddle + offset / zoomMultiplier;
            if (Physics.Raycast(desiredPos, playerMiddle - desiredPos, out hit, (playerMiddle - desiredPos).magnitude, wallLayers, QueryTriggerInteraction.Ignore) && Physics.Raycast(gork.transform.position, clide.transform.position - gork.transform.position, (clide.transform.position - gork.transform.position).magnitude, wallLayers, QueryTriggerInteraction.Ignore) == false)
            {
                targetPos = Vector3.Lerp(hit.point, playerMiddle, 0.7f);
            }
            else
            {
                targetPos = desiredPos;
            }
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);
        }
    }
}

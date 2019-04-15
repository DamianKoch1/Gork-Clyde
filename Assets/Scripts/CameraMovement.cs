using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player1, player2;
    private Vector3 offset;
    private Vector3 playerMiddle;
    [SerializeField]
    private float playerDistanceZoomThreshhold;
    private float zoomMultiplier;
    [SerializeField]
    private float minZoom, maxZoom, zoomSpeed, followSpeed, rotateSpeed = 2;
    void Start()
    {
        if (player1 != null && player2 != null)
        {
            playerMiddle = player1.transform.position + (0.5f * (player2.transform.position - player1.transform.position));
            offset = transform.position - playerMiddle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X")*rotateSpeed, Vector3.up) * offset;
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X")*rotateSpeed);
        }
        if (player1 != null && player2 != null)
        {
            playerMiddle = player1.transform.position + (0.5f * (player2.transform.position - player1.transform.position));
            zoomMultiplier = Mathf.Clamp((playerDistanceZoomThreshhold / (Vector3.Distance(player1.transform.position, player2.transform.position)))/zoomSpeed, minZoom, maxZoom);
            transform.position = Vector3.Lerp(transform.position, playerMiddle + offset / zoomMultiplier, followSpeed);
        }
    }
}

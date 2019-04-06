using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player1, player2;
    private Vector3 offset;
    private Vector3 playerMiddle;
    private float startPlayerDistance;
    private float zoomMultiplier;
    [SerializeField]
    private float minZoom, maxZoom, zoomSpeed;
    void Start()
    {
        if (player1 != null && player2 != null)
        {
            playerMiddle = player1.transform.position + (0.5f * (player2.transform.position - player1.transform.position));
            offset = transform.position - playerMiddle;
            startPlayerDistance = Vector3.Distance(player1.transform.position, player2.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 != null && player2 != null)
        {
            playerMiddle = player1.transform.position + (0.5f * (player2.transform.position - player1.transform.position));
            zoomMultiplier = Mathf.Clamp((startPlayerDistance / (Vector3.Distance(player1.transform.position, player2.transform.position)))/zoomSpeed, minZoom, maxZoom);
            transform.position = playerMiddle + offset / zoomMultiplier;
        }
    }
}

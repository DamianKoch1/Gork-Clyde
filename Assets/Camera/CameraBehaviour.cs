using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject gork;

    [SerializeField]
    private GameObject clyde;
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

    public static Animator ANIM;
    public static string NEXT_SCENE_NAME;
    
    void Start()
    {
        if (gork && clyde)
        {
            playerMiddle = 0.5f * (gork.transform.position + clyde.transform.position);
            offset = transform.position - playerMiddle;
        }
        ANIM = GetComponent<Animator>();
    }

    public static void FADE_TO_BLACK()
    {
        ANIM.SetTrigger("fadeToBlack");
    }

    public void OnBlackFadeFinished()
    {
        SceneManager.LoadScene(NEXT_SCENE_NAME);
    }
        
    
    private void Update()
    {
        //toggling pause/options menu
        if (Input.GetButtonDown("Cancel"))
        {
            var optionsCanvas = optionsMenu.GetComponent<Canvas>();
            var pauseCanvas = pauseMenu.GetComponent<Canvas>();
            if (!pauseCanvas.enabled)
            {
                pauseCanvas.enabled = true;
                Time.timeScale = 0;
            }
            else if (optionsCanvas.enabled)
            {
                optionsCanvas.enabled = false;
            }
            else
            {
                pauseCanvas.enabled = false;
                Time.timeScale = 1;
            }
        }
    }

    private void FixedUpdate()
    {
        //camera rotation (cutted, more for testing)
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
        //
        if (gork && clyde)
        {
            //zoom if players too far from each other
            zoomMultiplier = Mathf.Clamp((playerDistanceZoomThreshhold / (Vector3.Distance(gork.transform.position, clyde.transform.position)))/zoomSpeed, minZoom, maxZoom);
            
            //if player cant move focus other player
            if (!clyde.GetComponent<Player>().canMove)
            {
                desiredPos = gork.transform.position + offset / zoomMultiplier;
            }
            else if (!gork.GetComponent<Player>().canMove)
            {
                desiredPos = clyde.transform.position + offset / zoomMultiplier;
            }
            else
            {
                playerMiddle = 0.5f * (gork.transform.position + clyde.transform.position);
                desiredPos = playerMiddle + offset / zoomMultiplier;
            }
            
            //tried raycasting to prevent wall collision, hard to implement when camera focuses empty point
//          if (Physics.Raycast(desiredPos, playerMiddle - desiredPos, out hit, (playerMiddle - desiredPos).magnitude, wallLayers, QueryTriggerInteraction.Ignore) && Physics.Raycast(gork.transform.position, clide.transform.position - gork.transform.position, (clide.transform.position - gork.transform.position).magnitude, wallLayers, QueryTriggerInteraction.Ignore) == false)
//          {
//             targetPos = Vector3.Lerp(hit.point, playerMiddle, 0.7f);
//          }
//          else
//          {
            targetPos = desiredPos;
//          }
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);
        }
    }
}

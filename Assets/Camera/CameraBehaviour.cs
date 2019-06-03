using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject gork, clyde;

    private Vector3 offset;
    private Vector3 playerMiddle;
    private float startPlayerDistance, playerDistance;

    [SerializeField]
    private float playerDistanceZoomThreshhold = 2;
    private float zoomMultiplier;
    [SerializeField]
    private float minZoom, maxZoom;
    [SerializeField]
    private float followSpeed, rotateSpeed = 2;
    private Vector3 targetPos;
    private RaycastHit hit;

    [SerializeField]
    private GameObject pauseMenu, optionsMenu;

    private static Animator ANIM;
    public static string NEXT_SCENE_NAME;

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        RotateCamera();
        MoveCamera();
    }
    
    private void InitializeVariables()
    {
        playerMiddle = 0.5f * (gork.transform.position + clyde.transform.position);
        offset = transform.position - playerMiddle;
        startPlayerDistance = Vector3.Distance(gork.transform.position, clyde.transform.position);
        playerDistance = startPlayerDistance;
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

    private void CheckInput()
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
                MenuButton.FocusNothing();
            }
        }

        if (Input.GetButtonDown("DebugRestart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //cutted, kept for debugging
    private void RotateCamera()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, Vector3.up) * offset;
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed);
        }
        if (Input.GetAxis("Mouse Y") != 0)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotateSpeed, -transform.right) * offset;
            transform.RotateAround(transform.position, -transform.right, Input.GetAxis("Mouse Y") * rotateSpeed);
        }
    }

    private void MoveCamera()
    {
        playerDistance = Vector3.Distance(gork.transform.position, clyde.transform.position);

        //zoom if players too far from each other
        zoomMultiplier = Mathf.Clamp((playerDistanceZoomThreshhold * startPlayerDistance / playerDistance), minZoom, maxZoom);

        //if player cant move focus other player
        if (!clyde.GetComponent<Player>().canMove)
        {
            targetPos = gork.transform.position + offset / zoomMultiplier;
        }
        else if (!gork.GetComponent<Player>().canMove)
        {
            targetPos = clyde.transform.position + offset / zoomMultiplier;
        }
        else
        {
            playerMiddle = 0.5f * (gork.transform.position + clyde.transform.position);
            targetPos = playerMiddle + offset / zoomMultiplier;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed);
    }
}

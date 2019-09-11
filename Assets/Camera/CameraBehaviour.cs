using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject gork, clyde;
    
    public static bool gorkCamEnabled = true, clydeCamEnabled = true;

    private Vector3 offset;
    private Vector3 playerMiddle;
    private float startPlayerDistance, playerDistance;
    [SerializeField]
    private CinemachineVirtualCamera gorkCam, clydeCam;

    /// <summary>
    /// max tolerated player distance before zoom = this * startPlayerDistance
    /// </summary>
    [SerializeField]
    private float playerDistanceZoomThreshhold = 2;
    private float zoomMultiplier;
    [SerializeField]
    private float minZoom, maxZoom;
    [SerializeField]
    private float followSpeed, rotateSpeed = 2;
    private Vector3 targetPos;

  


    private void Start()
    {
        InitializeVariables();
        GetComponent<CinemachineVirtualCamera>().Priority = 1;
    }

    private void FixedUpdate()
    {
        UpdatePlayerCameras();

        MoveCamera();
    }

    private void UpdatePlayerCameras()
    {
        UpdateGorkCam();
        UpdateClydeCam();
    }

    private void UpdateGorkCam()
    {
        if (!gorkCamEnabled)
        {
            gorkCam.Priority = 0;
            return;
        }
        if (gork.GetComponent<PlayerState>().canMove)
        {
            if (Input.GetButton(Gork.GorkCam))
            {
                gorkCam.Priority = 51;
            }
            else
            {
                gorkCam.Priority = 0;
            }
        }
    }
    
    private void UpdateClydeCam()
    {
        if (!clydeCamEnabled)
        {
            clydeCam.Priority = 0;
            return;
        }
        if (clyde.GetComponent<PlayerState>().canMove)
        {
            if (Input.GetButton(Clyde.ClydeCam))
            {
                clydeCam.Priority = 51;
            }
            else
            {
                clydeCam.Priority = 0;
            }
        }
    }
    
    
    private void InitializeVariables()
    {
        playerMiddle = 0.5f * (gork.transform.position + clyde.transform.position);
        offset = transform.position - playerMiddle;
        startPlayerDistance = Vector3.Distance(gork.transform.position, clyde.transform.position);
        playerDistance = startPlayerDistance;
    }

  
    /// <summary>
    /// Camera stays between players, zooms based on distance, if player can't move focuses other only
    /// </summary>
    private void MoveCamera()
    {
        playerDistance = Vector3.Distance(gork.transform.position, clyde.transform.position);

        zoomMultiplier = Mathf.Clamp((playerDistanceZoomThreshhold * startPlayerDistance / playerDistance), minZoom, maxZoom);

        if (!clyde.GetComponent<PlayerState>().canMove)
        {
            targetPos = gork.transform.position + offset / zoomMultiplier;
        }
        else if (!gork.GetComponent<PlayerState>().canMove)
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
    
    /// <summary>
    /// Rotates a vector to current camera perspective while keeping y value
    /// </summary>
    /// <param name="vector">vector to rotate</param>
    /// <returns>Returns rotated vector</returns>
    public static Vector3 ApplyCameraRotation(Vector3 vector)
    {
        Camera cam = Camera.main;
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();
        Vector3 rotatedVector = vector.x * camRight + vector.y * Vector3.up + vector.z * camForward;
        return rotatedVector;
    }
    
}

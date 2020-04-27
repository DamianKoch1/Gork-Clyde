using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private Player gork;

    [SerializeField]
    private Player clyde;


    public static bool gorkCamEnabled = true;
    public static bool clydeCamEnabled = true;

    private Vector3 offset;
    private Vector3 playerMiddle;
    private float startPlayerDistance;
    private float curPlayerDistance;

    [SerializeField]
    private CinemachineVirtualCamera gorkCam;

    [SerializeField]
    private CinemachineVirtualCamera clydeCam;

    [SerializeField, Tooltip("Max tolerated ratio of current player distance to start player distance before camera zooms out")]
    private float playerDistanceZoomThreshhold = 2;

    private float zoomMultiplier;

    [SerializeField]
    private float minZoom;

    [SerializeField]
    private float maxZoom;

    [SerializeField]
    private float followSpeed;

    [SerializeField]
    private float rotateSpeed = 2;

    private Vector3 targetPos;

  


    private void Start()
    {
        InitializeVariables();
        GetComponent<CinemachineVirtualCamera>().Priority = 1;
    }

    private void Update()
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
        if (gork.canMove)
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
        if (clyde.canMove)
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
        curPlayerDistance = startPlayerDistance;
    }

  
    /// <summary>
    /// Camera stays between players, zooms based on distance, if one player can't move focuses other only
    /// </summary>
    private void MoveCamera()
    {
        curPlayerDistance = Vector3.Distance(gork.transform.position, clyde.transform.position);

        zoomMultiplier = Mathf.Clamp((playerDistanceZoomThreshhold * startPlayerDistance / curPlayerDistance), minZoom, maxZoom);

        if (!clyde.canMove)
        {
            targetPos = gork.transform.position + offset / zoomMultiplier;
        }
        else if (!gork.canMove)
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

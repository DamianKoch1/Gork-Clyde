using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject start, target, platform;
    private Vector3 pos1, pos2, targetPos;
    private bool stop, blocked;
    public enum Mode { Autostart, Triggerable };
    public Mode mode = Mode.Autostart;
    [SerializeField]
    private float speed;
    private Rigidbody rb;
    private float moveAmount = 0;


    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        rb = platform.GetComponent<Rigidbody>();
        pos1 = start.transform.position;
        pos2 = target.transform.position;
        targetPos = pos2;
        if (mode != Mode.Autostart)
        {
            stop = true;
        }
    }


    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (stop) return;
        if (blocked) return;

        if (moveAmount >= Mathf.PI * 200)
        {
            moveAmount = 0;
        }
        rb.MovePosition(pos1 + 0.5f * (1 + Mathf.Sin(moveAmount - Mathf.PI / 2)) * (pos2 - pos1));
        moveAmount += Time.fixedDeltaTime * speed;
    }


    public void PlatformBlocked()
    {
        blocked = true;
    }

    public void PlatformUnblocked()
    {
        blocked = false;
    }

    public void OnButtonActivated()
    {
        stop = false;
    }

    public void OnPlateActivated()
    {
        stop = false;
    }
    public void OnPlateExited()
    {
        stop = true;
    }
}

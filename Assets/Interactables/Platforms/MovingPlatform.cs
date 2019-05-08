using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject start, target, platform;
    private Vector3 pos1, pos2, targetPos;
    private bool stop;
    public enum Mode {Autostart, Continuous, Oneshot};
    public Mode mode = Mode.Autostart;
    [SerializeField]
    private float speed;
    private Rigidbody rb;
    private float moveAmount = 0;

    void Start()
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stop == false)
        {
            if (moveAmount >= Mathf.PI * 20) moveAmount = 2 * Mathf.PI;
            rb.MovePosition(pos1 + 0.5f*(1+Mathf.Sin(moveAmount-Mathf.PI/2))*(pos2 - pos1));
            moveAmount += Time.fixedDeltaTime * speed;
            if (mode == Mode.Oneshot)
            {
                if ((Vector3.Distance(rb.position, targetPos) < 0.1f) && moveAmount > 0.5*Mathf.PI)
               {
                    stop = true;
                    if (targetPos == pos1)
                    {
                        targetPos = pos2;
                    }
                    else
                    {
                        targetPos = pos1;
                    }
               }
            }
        }
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

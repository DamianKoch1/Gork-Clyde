using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject start, target, platform;
    private Vector3 pos1, pos2;
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
        if (mode == Mode.Autostart)
        {
            stop = false;
        }
        else
        {
            stop = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveAmount += Time.fixedDeltaTime * speed;
        if (moveAmount >= Mathf.PI * 20) moveAmount = 0;
        
        if (stop == false)
        {
            rb.MovePosition(pos1 + 0.5f*(1+Mathf.Sin(moveAmount))*(pos2 - pos1));
            if ((Vector3.Distance(rb.position, pos1) < 0.1f) || (Vector3.Distance(rb.position, pos2) < 0.1f))
            {
               if (mode == Mode.Oneshot)
               {
                    stop = true;
               }
            }
        }
    }

    public void OnButtonActivated()
    {
        if (Mathf.Abs(Mathf.Sin(moveAmount)) < 0.45f)
        {
            stop = false;
        }
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

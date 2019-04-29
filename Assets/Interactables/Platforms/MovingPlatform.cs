using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject start, target;
    private Vector3 pos1, pos2;
    private bool stop;
    private Vector3 startPos;
    private Vector3 targetPos;
    public enum Mode {Autostart, Continuous, Oneshot, PressurePlate};
    public Mode mode = Mode.Autostart;
    [SerializeField]
    private float speed;
    private Rigidbody rb;
   

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pos1 = start.transform.position;
        startPos = transform.position;
        pos2 = target.transform.position;
        targetPos = pos1;
        if (mode == Mode.Autostart)
        {
            Move();
        }
        else if (mode == Mode.PressurePlate)
        {
            Move();
            stop = true;
        }
        else
        {
            stop = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stop == false)
        {
            rb.MovePosition(rb.position + (targetPos - startPos).normalized/10 * speed);
            if (Vector3.Distance(rb.position, targetPos) < 0.1f)
            {
               if (mode != Mode.Oneshot)
               {
                    Move();
               }
               else
               {
                    stop = true;
               }
            }
        }


    }

    void Move()
    {
        stop = false;
        if (targetPos == pos1)
        {
            targetPos = pos2;
        }
        else
        {
            targetPos = pos1;
        }
        startPos = transform.position;
    }

   
    public void OnButtonActivated()
    {
        if (Vector3.Distance(rb.position, targetPos) < 0.1f)
        {
            Move();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 pos1, pos2;
    private bool stop;
    private Vector3 startPos;
    private Vector3 targetPos;
    public enum Mode {Autostart, Continuous, Oneshot, PressurePlate}
    public Mode mode = Mode.Autostart;
    private float lerpAmount;
    [SerializeField]
    private float speed;
   
    void Start()
    {
        pos1 = transform.Find("Pos1").transform.position;
        startPos = transform.position;
        pos2 = transform.Find("Pos2").transform.position;
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
        lerpAmount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stop == false)
        {
            lerpAmount += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, lerpAmount);
            if (lerpAmount >= 1)
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
        lerpAmount = 0;
    }

   
    public void OnButtonActivated()
    {
        if (lerpAmount <= 0 || lerpAmount >= 1)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 pos1, pos2;
    private bool stop;
    private Vector3 startPos;
    private Vector3 targetPos;
    public enum Mode {autostart, triggeredContinuous, triggeredOneshot, moveWhileOnButton};
    public Mode mode = Mode.autostart;
    private float lerpAmount;
    [SerializeField]
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        pos1 = transform.Find("Pos1").transform.position;
        pos2 = transform.Find("Pos2").transform.position;
        targetPos = pos1;
        if (mode == Mode.autostart)
        {
            Move();
        } else if (mode == Mode.moveWhileOnButton)
        {
            Move();
            stop = true;
        } else
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
               if (mode != Mode.triggeredOneshot)
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
        if (targetPos == pos1)
        {
            targetPos = pos2;
        } else
        {
            targetPos = pos1;
        }
        startPos = transform.position;
        lerpAmount = 0;
        stop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform, true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent = transform)
        {
            other.transform.SetParent(null, true);
        }
    }
    public void OnButtonActivated()
    {
        if (mode == Mode.moveWhileOnButton)
        {
            stop = false;
        } else
        {
            if (lerpAmount <= 0 || lerpAmount >= 1)
            {
                Move();
            }
        }
    }
    public void OnButtonExited()
    {
        if (mode == Mode.moveWhileOnButton)
        {
            stop = true;
        }
    }
}

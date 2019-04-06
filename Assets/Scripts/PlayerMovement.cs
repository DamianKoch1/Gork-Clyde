using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private int playerID;
    [SerializeField]
    private float speed, jumpHeight, pushSpeed, fallSpeed;
    private Vector3 motion, pushMotion;
    private CharacterController cc;
    private string xAxis, zAxis, jumpButton;
    private bool inAirstream;
    private Vector3 airstreamMotion;

    void Start()
    {
        inAirstream = false;
        cc = GetComponent<CharacterController>();
        switch (playerID)
        {
            case 1:
                xAxis = "Horizontal";
                zAxis = "Vertical";
                jumpButton = "Jump";
                break;
            case 2:
                xAxis = "Horizontal2";
                zAxis = "Vertical2";
                jumpButton = "Jump2";
                break;
            default:
                break;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            if (cc.velocity.y == 0)
            {
                motion.y = 0;
                if (Input.GetButtonDown(jumpButton))
                {
                    motion.y = jumpHeight;
                }
            }
        } else
        {
            motion.y -= fallSpeed*Time.deltaTime;
        }
        motion.x = Input.GetAxis(xAxis);
        motion.z = Input.GetAxis(zAxis);
       
        motion.x *= speed;
        motion.z *= speed;
        if (inAirstream == false)
        {
            airstreamMotion *= 0.9f;
        }
            cc.Move((motion+airstreamMotion) * Time.deltaTime);
      
    }
    public void Bounce(object[] parameters)
    {
        if(motion.y < 0)
        {
            motion.y = Mathf.Min(motion.y *= -(float)parameters[0], (float)parameters[1]);
        }
    }

    public void ToggleAirstream(object[] parameters)
    {
        if ((bool)parameters[0] == false)
        {
            inAirstream = false;
        } else
        {
            inAirstream = true;
            airstreamMotion = (Vector3)parameters[1] * (float)parameters[2];
        }
        Debug.LogWarning(inAirstream);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("pushable"))
        {
            pushMotion.x = cc.velocity.x;
            pushMotion.y = 0;
            pushMotion.z = cc.velocity.z;
            hit.rigidbody.velocity = pushMotion * pushSpeed;

        }
    }

}


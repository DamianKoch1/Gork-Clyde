using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Gork : Player
{
    [SerializeField]
    private GameObject heldObjectSlot;
    private List<GameObject> carryableObjects = new List<GameObject>();
    [SerializeField]
    private float throwStrength = 5f;
    [SerializeField] [Range(0, 90)]
    private float throwUpwardsAngle = 20f;

    private FixedJoint fixedJoint;

    private bool pushing = false;
    

    public static string XAXIS = "GorkHorizontal", ZAXIS = "GorkVertical", JUMPBUTTON = "GorkJump", GORKINTERACT = "GorkInteract";

    private new void Start()
    {
        base.Start();
        InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
    }
    
    private new void Update()
    {
        base.Update();
        if (Input.GetButtonDown(GORKINTERACT))
        {
            if (heldObjectSlot.transform.childCount == 0)
            {
                if (carryableObjects.Count > 0)
                {
                    PickUp(carryableObjects[0]);
                }
            }
            else
            {
                Throw(heldObjectSlot.transform.GetChild(0).gameObject, ThrowDirection(), throwStrength);
            }
        }

        if (heldObjectSlot.transform.childCount > 0)
        {
            GameObject clyde = heldObjectSlot.transform.GetChild(0).gameObject;
            if (clyde.GetComponent<Clyde>())
            {
                if (Input.GetButtonDown(Clyde.JUMPBUTTON))
                {
                    clyde.GetComponent<Clyde>().CancelThrow();
                }
            }
        }
    }


    public void StartPushing(GameObject obj)
    {
        if (heldObjectSlot.transform.childCount == 0 && !pushing)
        {
            pushing = true;
            Rigidbody objectRb = obj.GetComponent<Rigidbody>();
            obj.layer = 2;
            Vector3 direction = objectRb.position - rb.position;
            AxisAlignToBox(direction);
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = objectRb;
        }
    }


    private void AxisAlignToBox(Vector3 vector)
    {
        vector.y = 0;
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
        {
            vector.z = 0;
            setMotion = SetMotionXOnly;
        }
        else
        {
            vector.x = 0;
            setMotion = SetMotionZOnly;
        }
        transform.LookAt(rb.position + vector);
    }
    
    public void EndPushing()
    {
        GameObject obj = fixedJoint.connectedBody.gameObject;
        obj.layer = 0;
        Destroy(fixedJoint);        
        setMotion = SetMotionDefault;
        pushing = false;
    }
    
    
    private void SetMotionXOnly()
    {
        motion.x = Input.GetAxis(xAxis);
        motion = motion.normalized * speed;
        if (Mathf.Abs(Input.GetAxis(zAxis)) > 0.1f)
        {
            EndPushing();
        }
    }
    
    private void SetMotionZOnly()
    {
        motion.z = Input.GetAxis(zAxis);
        motion = motion.normalized * speed;
        if (Mathf.Abs(Input.GetAxis(xAxis)) > 0.1f)
        {
            EndPushing();
        }
    }
    
    
    protected override void SetSpawnPoint()
    {
        Spawnpoint.GORK_SPAWN = rb.position;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody && !hit.rigidbody.isKinematic)
        {
            hit.rigidbody.AddForce(hit.moveDirection * Time.deltaTime * 60 * 30/hit.rigidbody.mass, ForceMode.VelocityChange);
        }
    }

    
    
    private void OnTriggerEnter(Collider other)
    {
        if (!carryableObjects.Contains(other.gameObject) && other.GetComponent<Carryable>())
        {
            carryableObjects.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (carryableObjects.Contains(other.gameObject))
        {
            carryableObjects.Remove(other.gameObject);
        }
    }
    private void PickUp(GameObject obj)
    {
        var clyde = obj.GetComponent<Clyde>();
        anim.SetTrigger("pickUp");
        Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
        obj.transform.SetParent(heldObjectSlot.transform, true);
        obj.transform.position = heldObjectSlot.transform.position;
        obj.transform.LookAt(obj.transform.position + transform.forward);
        Rigidbody objectRb = obj.GetComponent<Rigidbody>();
        objectRb.isKinematic = true;
        if (clyde)
        {
            clyde.canMove = false;
            clyde.anim.SetTrigger("pickedUp");
            clyde.gork = gameObject;

        }
    }
    
    private void Throw(GameObject obj, Vector3 direction, float strength)
    {
        GetComponent<AudioSource>().Play();
        var clyde = obj.GetComponent<Clyde>();
        anim.SetTrigger("throw");
        obj.transform.SetParent(null, true);
        Rigidbody objectRb = obj.GetComponent<Rigidbody>();
        if (clyde)
        {
            clyde.ResetMotion();
            clyde.canMove = true;
            clyde.anim.SetTrigger("thrown");
            clyde.anim.ResetTrigger("land");
        }
        objectRb.velocity = Vector3.zero;
        objectRb.isKinematic = false;
        objectRb.AddForce(direction*strength*Time.fixedDeltaTime*60, ForceMode.VelocityChange);
        objectRb = null;
        Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>(), false);
    }

    private Vector3 ThrowDirection()
    {
        Vector3 throwDirection = transform.forward;
        throwDirection = Quaternion.AngleAxis(throwUpwardsAngle, -transform.right) * throwDirection;
        return throwDirection;
    }
    
}

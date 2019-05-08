using System.Collections;
using System.Collections.Generic;
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
    private Rigidbody objectRb;

    private void Update()
    {
        base.Update();
        if (Input.GetButtonDown("GorkInteract"))
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
                Throw(heldObjectSlot.transform.GetChild(0).gameObject);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody != null && hit.rigidbody.isKinematic == false)
        {
            hit.rigidbody.AddForce(hit.moveDirection * Time.deltaTime * 60 * 30/hit.rigidbody.mass, ForceMode.VelocityChange);
        }
    }

    protected override void InitializeInputs()
    {
        xAxis = "GorkHorizontal";
        zAxis = "GorkVertical";
        jumpButton = "GorkJump";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (carryableObjects.Contains(other.gameObject) == false && other.GetComponent<Carryable>() != null)
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
        var clide = obj.GetComponent<Clyde>();
        anim.ResetTrigger("pickup");
        anim.SetTrigger("pickup");
        Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
        obj.transform.SetParent(heldObjectSlot.transform, true);
        obj.transform.position = heldObjectSlot.transform.position;
        obj.transform.LookAt(obj.transform.position + transform.forward);
        objectRb = obj.GetComponent<Rigidbody>();
        objectRb.isKinematic = true;
        if (clide != null)
        {
            clide.canMove = false;
        }
    }
    private void Throw(GameObject obj)
    {
        GetComponent<AudioSource>().Play();
        var clide = obj.GetComponent<Clyde>();
        anim.ResetTrigger("throw");
        anim.SetTrigger("throw");
        Vector3 throwDirection = transform.forward * throwStrength;
        throwDirection = Quaternion.AngleAxis(throwUpwardsAngle, -transform.right) * throwDirection;
        obj.transform.SetParent(null, true);
        if (clide != null)
        {
            clide.ResetMotion();
            clide.canMove = true;
        }
        objectRb.velocity = Vector3.zero;
        objectRb.isKinematic = false;
        objectRb.AddForce(throwDirection*Time.fixedDeltaTime*60, ForceMode.VelocityChange);
        objectRb = null;
        Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>(), false);
    }
}

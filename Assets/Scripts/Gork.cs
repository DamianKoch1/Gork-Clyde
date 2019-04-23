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

    protected override void InitializeInputs()
    {
        xAxis = "Horizontal";
        zAxis = "Vertical";
        jumpButton = "Jump";
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
        anim.ResetTrigger("pickup");
        anim.SetTrigger("pickup");
        objectRb = obj.GetComponent<Rigidbody>();
        objectRb.isKinematic = true;
        Physics.IgnoreCollision(GetComponent<Collider>(), objectRb.GetComponent<Collider>());
        obj.transform.SetParent(heldObjectSlot.transform, true);
        obj.transform.position = heldObjectSlot.transform.position;
        obj.transform.LookAt(obj.transform.position + transform.forward);
        objectRb.interpolation = RigidbodyInterpolation.None;
    }
    private void Throw(GameObject obj)
    {
        anim.ResetTrigger("throw");
        anim.SetTrigger("throw");
        objectRb.isKinematic = false;
        Vector3 throwDirection = transform.forward * throwStrength;
        throwDirection = Quaternion.AngleAxis(throwUpwardsAngle, -transform.right) * throwDirection;
        objectRb.velocity = Vector3.zero;
        obj.transform.SetParent(null, true);
        if (obj.GetComponent<Clide>() != null)
        {
            obj.GetComponent<Clide>().ResetMotion();
            obj.GetComponent<AirstreamAffected>().airstreamMotion = throwDirection * Time.deltaTime * 60 * 2;
        }
        else
        {
           objectRb.AddForce(throwDirection*Time.deltaTime*60, ForceMode.VelocityChange);
        }
        Physics.IgnoreCollision(GetComponent<Collider>(), objectRb.GetComponent<Collider>(), false);
        objectRb.interpolation = RigidbodyInterpolation.Interpolate;
        objectRb = null;
    }
}

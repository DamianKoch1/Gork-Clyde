using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThrowIndicator))]
public class Throwing : MonoBehaviour
{
    private List<GameObject> carryableObjects = new List<GameObject>();

    [HideInInspector]
    public Animator anim;
    
    [SerializeField]
    private float throwStrength = 16f;
	
    [SerializeField]
    private float throwBoxStrength = 16f;

    [SerializeField]
    [Range(0, 90)]
    private float throwUpwardsAngle = 50f;
	
    [SerializeField]
    [Range(0, 90)]
    private float throwBoxUpwardsAngle = 45f;
   
    private ThrowIndicator throwIndicator;	
    
    [SerializeField]
    private GameObject heldObjectSlot;
    
    // Start is called before the first frame update
    private void Start()
    {
	    throwIndicator = GetComponent<ThrowIndicator>();
    }

    // Update is called once per frame
    private void Update()
    {
	    UpdateThrowIndicator();
	    CheckForCancel();
    }

    /// <summary>
    /// Checks if clyde cancelled throw
    /// </summary>
    private void CheckForCancel()
    {
	    if (Input.GetButtonDown(Clyde.JumpButton))
	    {
		    if (IsCarryingObject())
		    {
			    var clyde = HeldObject().GetComponent<Clyde>();
			    if (clyde)
			    {
				    clyde.CancelThrow();
				    throwIndicator.DestroyIndicator();
			    }
		    }
	    }
    }
    
    /// <summary>
    /// Updates indicator when carrying object
    /// </summary>
    private void UpdateThrowIndicator()
    {
	    if (IsCarryingObject())
	    {
		    throwIndicator.UpdateIndicator(ThrowVector(), HeldObject());
	    }
	    else
	    {
		    throwIndicator.DestroyIndicator();
	    }
    }

    public bool IsCarryingObject()
    {
	    if (heldObjectSlot.transform.childCount > 0) return true;
	    return false;
    }

    public GameObject HeldObject()
    {
	    if (!IsCarryingObject()) return null;
	    return heldObjectSlot.transform.GetChild(0).gameObject;
    }

    public void Interact()
    {
	    if (IsCarryingObject())
	    {
		    Throw(HeldObject(), ThrowVector());
	    }
	    else if (carryableObjects.Count > 0)
	    {
		    PickUp(carryableObjects[0]);
	    }
    }
    
    private void OnTriggerEnter(Collider other)
    {
	    if (other.isTrigger) return;
	    if (carryableObjects.Contains(other.gameObject)) return;
	    if (!other.GetComponent<Carryable>()) return;

	    carryableObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
	    if (other.isTrigger) return;
	    if (!carryableObjects.Contains(other.gameObject)) return;

	    carryableObjects.Remove(other.gameObject);
    }
    
    public void PickUp(GameObject obj)
    {
	    if (IsCarryingObject()) return;
	    var clyde = obj.GetComponent<Clyde>();
	    if (clyde)
	    {
		    if (clyde.state.inAirstream) return;
		    clyde.state.canMove = false;
		    clyde.anim.SetTrigger("pickedUp");
	    }

	    obj.GetComponent<Carryable>().isHeld = true;
	    anim.SetTrigger("pickUp");
	    Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
	    obj.transform.SetParent(heldObjectSlot.transform, true);
	    obj.transform.position = heldObjectSlot.transform.position;
	    //obj.transform.LookAt(obj.transform.position + transform.forward);
	    Rigidbody objectRb = obj.GetComponent<Rigidbody>();
	    objectRb.isKinematic = true;
    }

    private void Throw(GameObject obj, Vector3 vector)
    {
	    var clyde = obj.GetComponent<Clyde>();
	    if (clyde)
	    {
		    clyde.ResetMotion();
		    clyde.state.canMove = true;
		    clyde.anim.SetTrigger("thrown");
		    clyde.anim.ResetTrigger("land");
		    clyde.RestartPickupCooldown();
		    clyde.state.canJumpTimeframe = 0;
		    clyde.state.isThrown = true;
	    }

	    obj.GetComponent<Carryable>().isHeld = false;
	    GetComponent<AudioSource>().Play();
	    anim.SetTrigger("throw");
	    obj.transform.SetParent(null, true);
	    Rigidbody objectRb = obj.GetComponent<Rigidbody>();
	    objectRb.velocity = Vector3.zero;
	    objectRb.isKinematic = false;
	    objectRb.AddForce(vector, ForceMode.VelocityChange);
	    Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>(), false);
	    throwIndicator.DestroyIndicator();
    }

    private Vector3 ThrowVector()
    {
	    Vector3 throwVector = transform.forward;
	    float angle = throwUpwardsAngle;
	    if (!HeldObject().GetComponent<Clyde>())
	    {
		    angle = throwBoxUpwardsAngle;
	    }
	    throwVector = Quaternion.AngleAxis(angle, -transform.right) * throwVector;

	    float throwStr = throwStrength;
	    if (!HeldObject().GetComponent<Clyde>())
	    {
		    throwStr = throwBoxStrength;
	    }
	    throwVector *= throwStr;
		
	    return throwVector;
    }
    
}

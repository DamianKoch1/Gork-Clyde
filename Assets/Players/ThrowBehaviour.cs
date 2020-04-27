using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThrowIndicator))]
public class ThrowBehaviour : MonoBehaviour
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

    [Header("SFX")] 
    [SerializeField] 
    private AudioSource sfxAudioSource;

    [SerializeField] 
    private AudioClip throwSFX;


    public GameObject HeldObject
    {
        get
        {
            if (!IsCarryingObject()) return null;
            return heldObjectSlot.transform.GetChild(0).gameObject;
        }
    }

    public void Initialize(Animator _anim)
    {
	    throwIndicator = GetComponent<ThrowIndicator>();
	    anim = _anim;
    }

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
			    var clyde = HeldObject.GetComponent<Clyde>();
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
		    throwIndicator.UpdateIndicator(CalculateThrowVector(), HeldObject);
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

   

    public void Interact()
    {
	    if (IsCarryingObject())
	    {
		    Throw(HeldObject, CalculateThrowVector());
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
		    if (clyde.inAirstream) return;
		    if (!clyde.canMove) return;
		    clyde.canMove = false;
		    clyde.anim.SetTrigger("pickedUp");
		    clyde.transform.LookAt(clyde.rb.position + transform.forward);
	    }

	    obj.GetComponent<Carryable>().isHeld = true;
	    anim.SetTrigger("pickUp");
	    Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
	    obj.transform.SetParent(heldObjectSlot.transform, true);
	    obj.transform.position = heldObjectSlot.transform.position;
	    Rigidbody objectRb = obj.GetComponent<Rigidbody>();
	    objectRb.isKinematic = true;
    }

    private void Throw(GameObject obj, Vector3 vector)
    {
	    var clyde = obj.GetComponent<Clyde>();
	    if (clyde)
	    {
		    clyde.ResetMotion();
		    clyde.canMove = true;
		    clyde.anim.SetTrigger("thrown");
		    clyde.anim.ResetTrigger("land");
		    clyde.RestartPickupCooldown();
		    clyde.canJumpTimeframe = 0;
		    clyde.isThrown = true;
	    }

	    obj.GetComponent<Carryable>().isHeld = false;
	    sfxAudioSource.PlayOneShot(throwSFX);
	    anim.SetTrigger("throw");
	    obj.transform.SetParent(null, true);
	    Rigidbody objectRb = obj.GetComponent<Rigidbody>();
	    objectRb.velocity = Vector3.zero;
	    objectRb.isKinematic = false;
	    objectRb.AddForce(vector, ForceMode.VelocityChange);
	    Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>(), false);
	    throwIndicator.DestroyIndicator();
    }

    private Vector3 CalculateThrowVector()
    {
	    Vector3 throwVector = transform.forward;
	    float angle = throwUpwardsAngle;
        var clyde = HeldObject.GetComponent<Clyde>();
	    if (!clyde)
	    {
		    angle = throwBoxUpwardsAngle;
	    }
	    throwVector = Quaternion.AngleAxis(angle, -transform.right) * throwVector;

	    float throwStr = throwStrength;
	    if (!clyde)
	    {
		    throwStr = throwBoxStrength;
	    }
	    throwVector *= throwStr;
		
	    return throwVector;
    }
    
}

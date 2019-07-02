using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gork : Player
{
	private List<GameObject> carryableObjects = new List<GameObject>();

	[SerializeField]
	private float throwStrength = 12f;
	
	[SerializeField]
	private float throwBoxStrength = 15f;

	[SerializeField]
	[UnityEngine.Range(0, 90)]
	private float throwUpwardsAngle = 50f;
	
	[SerializeField]
	[UnityEngine.Range(0, 90)]
	private float throwBoxUpwardsAngle = 60f;

	private ThrowIndicator throwIndicator;	

	private FixedJoint fixedJoint;

	[HideInInspector]
	public GameObject pushedObj;

	private bool pushing;

	[SerializeField]
	private GameObject heldObjectSlot;
	
	
	
	public static string XAXIS = "GorkHorizontal",
	ZAXIS = "GorkVertical",
	JUMPBUTTON = "GorkJump",
	GORKINTERACT = "GorkInteract";


	protected override void Start()
	{
		base.Start();
		InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
		throwIndicator = GetComponent<ThrowIndicator>();
	}

	protected override void Update()
	{
		base.Update();
		if (pushing)
		{
			CheckIfStillPushing();
		}

		if (IsCarryingObject())
		{
			throwIndicator.UpdateIndicator(ThrowVector(), HeldObject());
		}
		else
		{
			throwIndicator.DestroyIndicator();
		}
	}

	

	private void CheckIfStillPushing()
	{
		if (!fixedJoint)
		{
			StopPushing();
		}
		else if (!IsGrounded())
		{
			StopPushing();
		}
	}

	protected override void CheckInput()
	{
		base.CheckInput();

		if (Input.GetButtonDown(GORKINTERACT))
		{
			if (pushing)
			{
				StopPushing();
			}
			else if (IsCarryingObject())
			{
				Throw(HeldObject(), ThrowVector());
			}
			else if (carryableObjects.Count > 0)
			{
				PickUp(carryableObjects[0]);
			}
		}

		if (Input.GetButtonUp(GORKINTERACT))
		{
			if (pushedObj)
			{
				if (!IsCarryingObject())
				{
					StartPushing();
				}
			}
		}

		if (Input.GetButtonDown(Clyde.JUMPBUTTON))
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


	private bool IsCarryingObject()
	{
		if (heldObjectSlot.transform.childCount > 0) return true;
		return false;
	}

	private GameObject HeldObject()
	{
		if (!IsCarryingObject()) return null;
		return heldObjectSlot.transform.GetChild(0).gameObject;
	}

	private void StartPushing()
	{
		if (IsCarryingObject()) return;
		if (pushing) return;
		if (fixedJoint) return;

        anim.SetBool("push", true);
		pushing = true;
		Rigidbody objectRb = pushedObj.GetComponent<Rigidbody>();
		pushedObj.layer = 2;
		Vector3 direction = objectRb.position - rb.position;
		AxisAlignTo(direction);
		fixedJoint = gameObject.AddComponent<FixedJoint>();
		fixedJoint.connectedBody = objectRb;
		fixedJoint.breakForce = 800f; //TODO maybe use rb mass instead of hardcoding
		pushedObj.GetComponent<PushableBig>().isPushed = true;
	}


	private void AxisAlignTo(Vector3 vector)
	{
		var rotatedVector = ApplyCamRotation(vector);
		vector.y = 0;
		if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
		{
			vector.z = 0;
		}
		else
		{
			vector.x = 0;
		}
		transform.LookAt(rb.position + vector);
		ResetMotion();
		setMotion = SetMotionSingleAxis;
	}

	private void StopPushing()
	{
		pushedObj.layer = 0;
		if (fixedJoint)
		{
			Destroy(fixedJoint);
		}

		anim.SetBool("push", false);
		setMotion = SetMotionDefault;
		pushing = false;
		pushedObj.GetComponent<PushableBig>().isPushed = false;
		pushedObj = null;
	}


	private void SetMotionSingleAxis()
	{
		var forward = transform.forward;
		var camRotatedForward = ApplyCamRotation(forward);
		if (Mathf.Abs(camRotatedForward.z) > Mathf.Abs(camRotatedForward.x))
		{
			motion = forward * Input.GetAxis(zAxis) * speed;
			if (InverseVerticalPushControls(forward, camRotatedForward))
			{
				motion *= -1;
			}
			if (Mathf.Abs(Input.GetAxis(xAxis)) > 0.5f)
			{
				StopPushing();
			}
		}
		else
		{
			motion = forward * Input.GetAxis(xAxis) * speed;
			if (InverseHorizontalPushControls(forward, camRotatedForward))
			{
				motion *= -1;
			}
			if (Mathf.Abs(Input.GetAxis(zAxis)) > 0.5f)
			{
				StopPushing();
			}
		}
		motion.y = 0;
	}

	private bool InverseVerticalPushControls(Vector3 forward, Vector3 camRotatedForward)
	{
		if (Mathf.Abs(camRotatedForward.z) < Mathf.Abs(camRotatedForward.x)) return false;
		if (camRotatedForward.z > 0)
		{
			if (Mathf.Abs(forward.x) > Mathf.Abs(forward.z)) return true;
		}
		else
		{
			if (Mathf.Abs(forward.z) > Mathf.Abs(forward.x)) return true;
		}
		return false;
	}

	private bool InverseHorizontalPushControls(Vector3 forward, Vector3 camRotatedForward)
	{
		if (Mathf.Abs(camRotatedForward.x) < Mathf.Abs(camRotatedForward.z)) return false;
		if (camRotatedForward.x > 0)
		{
			if (Mathf.Abs(forward.z) > Mathf.Abs(forward.x)) return true;
		}
		else
		{
			if (Mathf.Abs(forward.x) > Mathf.Abs(forward.z)) return true;
		}
		return false;
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

	public bool PickUp(GameObject obj)
	{
        if (IsCarryingObject()) return false;
		var clyde = obj.GetComponent<Clyde>();
		if (clyde)
		{
			if (clyde.inAirstream) return false;
			clyde.canMove = false;
			clyde.anim.SetTrigger("pickedUp");
		}

		anim.SetTrigger("pickUp");
		Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
		obj.transform.SetParent(heldObjectSlot.transform, true);
		obj.transform.position = heldObjectSlot.transform.position;
		obj.transform.LookAt(obj.transform.position + transform.forward);
		Rigidbody objectRb = obj.GetComponent<Rigidbody>();
		objectRb.isKinematic = true;
        return true;
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
            clyde.ghostjumpTimer = 0;
            clyde.isThrown = true;
		}

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
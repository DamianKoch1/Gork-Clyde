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

	[SerializeField] 
	[Range(10, 50)] 
	private int throwIndicatorCount = 20;

	[SerializeField] 
	[Range(0.06f, 0.1f)] 
	private float throwIndicatorDetail = 0.08f;

	private FixedJoint fixedJoint;

	[HideInInspector]
	public GameObject pushedObj;

	private bool pushing;

	private GameObject landingIndicator;
	private LineRenderer throwLineRenderer;

	[Header("References")]
	[SerializeField]
	private GameObject landingIndicatorPrefab;
	[SerializeField]
	private GameObject heldObjectSlot, throwIndicator;
	
	
	
	public static string XAXIS = "GorkHorizontal",
	ZAXIS = "GorkVertical",
	JUMPBUTTON = "GorkJump",
	GORKINTERACT = "GorkInteract";


	protected override void Start()
	{
		base.Start();
		InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
		throwLineRenderer = throwIndicator.GetComponent<LineRenderer>();
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
			UpdateThrowIndicator();
		}
		else
		{
			DeleteThrowIndicator();
		}
	}

	private void UpdateThrowIndicator()
	{
		float amplifier = 1;
		
		float throwStr = throwStrength;
		
		if (!HeldObject().GetComponent<Clyde>())
		{
			amplifier = 0.5f;
			throwStr = throwBoxStrength;
		}

		Vector3 PointPosAtTime (float time) 
		{
			return throwIndicator.transform.position + ThrowDirection() * throwStr * time + Physics.gravity * time * time * amplifier;
		}

		throwLineRenderer.positionCount = throwIndicatorCount;
		for (int i = 0; i < throwLineRenderer.positionCount; i++)
		{
			var pointPosition = PointPosAtTime(i * throwIndicatorDetail);

			if (i > 3)
			{
				for (float f = 0; f < 1; f += 0.2f)
				{
					var pos = PointPosAtTime((i + f) * throwIndicatorDetail);
					if (Physics.OverlapSphere(pos, 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore)
						.Length > 0)
					{
						DeleteThrowIndicator(i+1, false);
						throwLineRenderer.SetPosition(i, pos);
						if (!landingIndicator)
						{
							landingIndicator = Instantiate(landingIndicatorPrefab);
						}
						landingIndicator.transform.position = pos;
						return;
					}
				}
			}
			
			throwLineRenderer.SetPosition(i, pointPosition);
		}
	}

	private void DeleteThrowIndicator(int from = 0, bool destroyLandingIndicator = true)
	{
		throwLineRenderer.positionCount = from;
		if (destroyLandingIndicator)
		{
			if (landingIndicator)
			{
				Destroy(landingIndicator);
			}
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
				Throw(HeldObject(), ThrowDirection());
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
					DeleteThrowIndicator();
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
		AxisAlignToBox(direction);
		fixedJoint = gameObject.AddComponent<FixedJoint>();
		fixedJoint.connectedBody = objectRb;
		fixedJoint.breakForce = 800f; //TODO maybe use rb mass instead of hardcoding
		pushedObj.GetComponent<PushableBig>().isPushed = true;
	}


	private void AxisAlignToBox(Vector3 vector)
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
			if (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
			{
				return true;
			}
		}
		else
		{
			if (Mathf.Abs(forward.z) > Mathf.Abs(forward.x))
			{
				return true;
			}
		}
		return false;
	}

	private bool InverseHorizontalPushControls(Vector3 forward, Vector3 camRotatedForward)
	{
		if (Mathf.Abs(camRotatedForward.x) < Mathf.Abs(camRotatedForward.z)) return false;
		if (camRotatedForward.x > 0)
		{
			if (Mathf.Abs(forward.z) > Mathf.Abs(forward.x))
			{
				return true;
			}
		}
		else
		{
			if (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
			{
				return true;
			}
		}
		return false;
	}

	protected override void SetSpawnPoint()
	{
		Spawnpoint.GORK_SPAWN = rb.position;
	}

	public override void Respawn()
	{
		base.Respawn();
		rb.MovePosition(Spawnpoint.GORK_SPAWN);
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

	private void Throw(GameObject obj, Vector3 direction)
	{
		var clyde = obj.GetComponent<Clyde>();
		float throwStr;
		if (clyde)
		{
			throwStr = throwStrength;
			clyde.ResetMotion();
			clyde.canMove = true;
			clyde.anim.SetTrigger("thrown");
			clyde.anim.ResetTrigger("land");
            clyde.RestartPickupCooldown();
            clyde.ghostjumpTimer = 0;
            clyde.isThrown = true;
		}
		else
		{
			throwStr = throwBoxStrength;
		}

		GetComponent<AudioSource>().Play();
		anim.SetTrigger("throw");
		obj.transform.SetParent(null, true);
		Rigidbody objectRb = obj.GetComponent<Rigidbody>();
		objectRb.velocity = Vector3.zero;
		objectRb.isKinematic = false;
		objectRb.AddForce(direction * throwStr, ForceMode.VelocityChange);
		objectRb = null;
		Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>(), false);
		DeleteThrowIndicator();
	}

	private Vector3 ThrowDirection()
	{
		Vector3 throwDirection = transform.forward;
		float angle = throwUpwardsAngle;
		if (!HeldObject().GetComponent<Clyde>())
		{
			angle = throwBoxUpwardsAngle;
		}
		throwDirection = Quaternion.AngleAxis(angle, -transform.right) * throwDirection;
		return throwDirection;
	}
}
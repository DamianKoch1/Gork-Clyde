using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gork : Player
{
	[SerializeField]
	private GameObject heldObjectSlot, throwIndicator;

	private List<GameObject> carryableObjects = new List<GameObject>();

	[SerializeField]
	private float throwStrength = 5f;

	[SerializeField]
	[Range(0, 90)]
	private float throwUpwardsAngle = 20f;

	private FixedJoint fixedJoint;

	[HideInInspector]
	public GameObject pushedObj;

	private bool pushing;

	[SerializeField]
	private GameObject throwIndicatorPoint;

	private GameObject[] throwIndicatorPoints = new GameObject[30];


	public static string XAXIS = "GorkHorizontal",
	ZAXIS = "GorkVertical",
	JUMPBUTTON = "GorkJump",
	GORKINTERACT = "GorkInteract";


	protected override void Start()
	{
		base.Start();
		InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
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
	}

	private void UpdateThrowIndicator()
	{
		//indicator: sin(angle) * _/2 - x^2 / throwstrength
		for (int i = 0; i < throwIndicatorPoints.Length; i++)
		{
			float x = i / 3.0f;
			float pointHeight;
			Vector3 pointPosition = x * throwStrength * 0.1f * transform.forward.normalized;

			pointHeight = - x * x + x;
			pointHeight += 10* x * Mathf.Sin(Mathf.Deg2Rad * throwUpwardsAngle);
			pointPosition.y = pointHeight;

			if (!throwIndicatorPoints[i])
			{
				throwIndicatorPoints[i] = Instantiate(throwIndicatorPoint, throwIndicator.transform);
			}

			throwIndicatorPoints[i].transform.position = throwIndicator.transform.position + pointPosition;
		}
	}

	private void DeleteThrowIndicator()
	{
		for (int i = 0; i < throwIndicatorPoints.Length; i++)
		{
			if (throwIndicatorPoints[i])
			{
				Destroy(throwIndicatorPoints[i]);
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
				Throw(HeldObject(), ThrowDirection(), throwStrength);
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
		ResetMotion();
	}

	private void StopPushing()
	{
		pushedObj.layer = 0;
		if (fixedJoint)
		{
			Destroy(fixedJoint);
		}

		setMotion = SetMotionDefault;
		pushing = false;
		pushedObj.GetComponent<PushableBig>().isPushed = false;
		pushedObj = null;
	}


	private void SetMotionXOnly()
	{
		motion.x = Input.GetAxis(xAxis);
		motion *= speed;
		if (Mathf.Abs(Input.GetAxis(zAxis)) > 0.3f)
		{
			StopPushing();
		}
	}

	private void SetMotionZOnly()
	{
		motion.z = Input.GetAxis(zAxis);
		motion *= speed;
		if (Mathf.Abs(Input.GetAxis(xAxis)) > 0.3f)
		{
			StopPushing();
		}
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

	private void PickUp(GameObject obj)
	{
		var clyde = obj.GetComponent<Clyde>();
		if (clyde)
		{
			if (clyde.inAirstream) return;
			clyde.canMove = false;
			clyde.anim.SetTrigger("pickedUp");
			clyde.gork = gameObject;
		}

		anim.SetTrigger("pickUp");
		Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
		obj.transform.SetParent(heldObjectSlot.transform, true);
		obj.transform.position = heldObjectSlot.transform.position;
		obj.transform.LookAt(obj.transform.position + transform.forward);
		Rigidbody objectRb = obj.GetComponent<Rigidbody>();
		objectRb.isKinematic = true;
	}

	private void Throw(GameObject obj, Vector3 direction, float strength)
	{
		var clyde = obj.GetComponent<Clyde>();
		if (clyde)
		{
			clyde.ResetMotion();
			clyde.canMove = true;
			clyde.anim.SetTrigger("thrown");
			clyde.anim.ResetTrigger("land");
		}

		GetComponent<AudioSource>().Play();
		anim.SetTrigger("throw");
		obj.transform.SetParent(null, true);
		Rigidbody objectRb = obj.GetComponent<Rigidbody>();
		objectRb.velocity = Vector3.zero;
		objectRb.isKinematic = false;
		objectRb.AddForce(direction * strength, ForceMode.VelocityChange);
		objectRb = null;
		Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>(), false);
		DeleteThrowIndicator();
	}

	private Vector3 ThrowDirection()
	{
		Vector3 throwDirection = transform.forward;
		throwDirection = Quaternion.AngleAxis(throwUpwardsAngle, -transform.right) * throwDirection;
		return throwDirection;
	}
}
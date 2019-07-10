using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorMath;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Throwing))]
public class Gork : Player
{
	[HideInInspector]
	public Throwing throwing;	

	private FixedJoint fixedJoint;

	[HideInInspector]
	public GameObject pushedObj;

	private bool pushing;
	
	public static string XAXIS = "GorkHorizontal",
	ZAXIS = "GorkVertical",
	JUMPBUTTON = "GorkJump",
	GORKINTERACT = "GorkInteract";


	protected override void Start()
	{
		base.Start();
		InitializeInputs(XAXIS, ZAXIS, JUMPBUTTON);
		throwing = GetComponent<Throwing>();
		throwing.anim = anim;
	}

	protected override void Update()
	{
		base.Update();
		if (pushing)
		{
			CheckIfStillPushing();
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
			Interact();
		}

		if (Input.GetButtonUp(GORKINTERACT))
		{
			if (pushedObj)
			{
				if (!throwing.IsCarryingObject())
				{
					StartPushing();
				}
			}
		}
	}

	private void Interact()
	{
		if (pushing)
		{
			StopPushing();
		}
		else
		{
			throwing.Interact();
		}
	}
	
	
	private void StartPushing()
	{
		if (throwing.IsCarryingObject()) return;
		if (pushing) return;
		if (fixedJoint) return;

        anim.SetBool("push", true);
		pushing = true;
		Rigidbody objectRb = pushedObj.GetComponent<Rigidbody>();
		AxisAlignTo(transform, objectRb);
		ResetMotion();
		setMotion = SetMotionSingleAxis;
		pushedObj.layer = 2;
		pushedObj.GetComponent<PushableBig>().isPushed = true;
		AddFixedJoint(objectRb);
	}

	private void AddFixedJoint(Rigidbody target)
	{
		fixedJoint = gameObject.AddComponent<FixedJoint>();
		fixedJoint.connectedBody = target;
		fixedJoint.breakForce = 800f; //TODO maybe use rb mass instead of hardcoding
	}
	
	private void StopPushing()
	{
		if (fixedJoint)
		{
			Destroy(fixedJoint);
		}

		if (pushedObj)
		{
			pushedObj.layer = 0;
			pushedObj.GetComponent<PushableBig>().isPushed = false;
			pushedObj = null;
		}

		anim.SetBool("push", false);
		pushing = false;
		ResetMotion();
		setMotion = SetMotionDefault;
	}


	private void SetMotionSingleAxis()
	{
		var forward = transform.forward;
		var camRotatedForward = ApplyCameraRotation(forward);
		if (!AlignsToXAxis(camRotatedForward))
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
		if (AlignsToXAxis(camRotatedForward)) return false;
		if (camRotatedForward.z > 0)
		{
			if (AlignsToXAxis(forward)) return true;
		}
		else
		{
			if (!AlignsToXAxis(forward)) return true;
		}
		return false;
	}

	private bool InverseHorizontalPushControls(Vector3 forward, Vector3 camRotatedForward)
	{
		if (!AlignsToXAxis(camRotatedForward)) return false;
		if (camRotatedForward.x > 0)
		{
			if (!AlignsToXAxis(forward)) return true;
		}
		else
		{
			if (AlignsToXAxis(forward)) return true;
		}
		return false;
	}
}
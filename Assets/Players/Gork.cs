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
	
	public static string XAxis = "GorkHorizontal",
	ZAxis = "GorkVertical",
	JumpButton = "GorkJump",
	GorkInteract = "GorkInteract";


	protected override void Start()
	{
		base.Start();
		InitializeInputs(XAxis, ZAxis, JumpButton);
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

		if (Input.GetButtonDown(GorkInteract))
		{
			Interact();
		}

		if (Input.GetButtonUp(GorkInteract))
		{
			if (pushing)
			{
				StopPushing();
			}
		}
	}

	private void Interact()
	{
		if (!pushing)
		{
			if (!throwing.IsCarryingObject())
			{
				StartPushing();
			}
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
		if (!pushedObj) return;

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
		fixedJoint.breakForce = 8000f; //TODO maybe use rb mass instead of hardcoding
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
		var x = Input.GetAxis(xAxis);
		var z = Input.GetAxis(zAxis);
		if (Mathf.Abs(x) > Mathf.Abs(z))
		{
			motion.x = x;
			motion.z = 0;
		}
		else
		{
			motion.x = 0;
			motion.z = z;
		}
		motion *= speed;
		motion = ApplyCameraRotation(motion);
		motion.y = 0;
	}

}
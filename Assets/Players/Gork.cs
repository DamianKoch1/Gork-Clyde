using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Throwing))]
public class Gork : Player
{
	[HideInInspector]
	public Throwing throwing;	

	private FixedJoint fixedJoint;

	[HideInInspector]
	public GameObject pushedObj;

	[HideInInspector]
	public bool pushing;
	
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

	/// <summary>
	/// Stops pushing if fixed joint broke or if falling
	/// </summary>
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

	/// <summary>
	/// Pickup / throw / push depending on current state
	/// </summary>
	private void Interact()
	{
		if (pushedObj)
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
	
	/// <summary>
	/// Sets up pushing: adds fixed joint, changes movement, starts push animation
	/// </summary>
	private void StartPushing()
	{
		if (throwing.IsCarryingObject()) return;
		if (pushing) return;
		if (fixedJoint) return;
		if (!pushedObj) return;

        anim.SetBool("push", true);
		pushing = true;
		Rigidbody objectRb = pushedObj.GetComponent<Rigidbody>();

		
		
		ResetMotion();
		setMotion = SetMotionPushing;
		//pushedObj.layer = 2;
		pushedObj.GetComponent<PushableBig>().isPushed = true;
		AddFixedJoint(objectRb);
	}

	/// <summary>
	/// Adds fixed joint connecting Gork with target, adapts breakForce to target mass
	/// </summary>
	/// <param name="target">RigidBody to connect joint with</param>
	private void AddFixedJoint(Rigidbody target)
	{
		fixedJoint = gameObject.AddComponent<FixedJoint>();
		fixedJoint.connectedBody = target;
		fixedJoint.breakForce = rb.mass * 400;
	}
	
	/// <summary>
	/// Reverts movement, destroys fixedJoint if still there, stops push animation
	/// </summary>
	private void StopPushing()
	{
		if (fixedJoint)
		{
			Destroy(fixedJoint);
		}

		if (pushedObj)
		{
			//pushedObj.layer = 0;
			pushedObj.GetComponent<PushableBig>().isPushed = false;
			pushedObj = null;
		}

		anim.SetBool("push", false);
		pushing = false;
		ResetMotion();
		setMotion = SetMotionDefault;
	}

	/// <summary>
	/// Disables rotating to motion vector
	/// </summary>
	private void SetMotionPushing()
	{
		motion.x = Input.GetAxis(xAxis);
		motion.z = Input.GetAxis(zAxis);
		
		motion *= speed;
		motion = CameraBehaviour.ApplyCameraRotation(motion);
	}

	
	
}
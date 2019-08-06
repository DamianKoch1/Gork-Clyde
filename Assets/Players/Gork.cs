using UnityEngine;

[RequireComponent(typeof(Throwing))]
[RequireComponent(typeof(BoxCollider))]
public class Gork : Player
{
	/// <summary>
	/// Handles everything related to throwing objects
	/// </summary>
	[HideInInspector]
	public Throwing throwing;

	public static string XAxis = "GorkHorizontal",
	ZAxis = "GorkVertical",
	JumpButton = "GorkJump",
	GorkInteract = "GorkInteract",
	GorkCam = "GorkCam";


	protected override void Start()
	{
		base.Start();
		InitializeInputs(XAxis, ZAxis, JumpButton);
		throwing = GetComponent<Throwing>();
		throwing.Initialize(anim);
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
			if (pushing.isPushing)
			{
				pushing.StopPushing();
			}
		}
	}

	/// <summary>
	/// Pickup / throw / push depending on current state
	/// </summary>
	private void Interact()
	{
		if (pushing.pushedObj)
		{
			if (!throwing.IsCarryingObject())
			{
				pushing.StartPushing();
			}
		}
		else
		{
			throwing.Interact();
		}
	}
}
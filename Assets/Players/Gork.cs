using UnityEngine;

[RequireComponent(typeof(ThrowBehaviour))]
[RequireComponent(typeof(BoxCollider))]
public class Gork : Player
{
	/// <summary>
	/// Handles everything related to throwing objects
	/// </summary>
	public ThrowBehaviour throwBehaviour { private set; get; }

    public static string XAxis = "GorkHorizontal";
    public static string ZAxis = "GorkVertical";
    public static string JumpButton = "GorkJump";
    public static string GorkInteract = "GorkInteract";
	public static string GorkCam = "GorkCam";


	protected override void Start()
	{
		base.Start();
		InitializeInputs(XAxis, ZAxis, JumpButton);
		throwBehaviour = GetComponent<ThrowBehaviour>();
		throwBehaviour.Initialize(anim);
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
			if (pushBehaviour.isPushing)
			{
				pushBehaviour.StopPushing();
			}
		}
		
	}

	/// <summary>
	/// Pickup / throw / push depending on current state
	/// </summary>
	private void Interact()
	{
		if (pushBehaviour.pushedObj)
		{
			if (!throwBehaviour.IsCarryingObject())
			{
				pushBehaviour.StartPushing();
			}
		}
		else
		{
			throwBehaviour.Interact();
		}
	}
}
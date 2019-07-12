using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMath
{
	/// <summary>
	/// Checks if a vector aligns closer to x axis than to z axis
	/// </summary>
	/// <param name="vector">vector to check</param>
	/// <returns>Returns true if vectors absolute x is bigger than z</returns>
	public static bool AlignsToXAxis(Vector3 vector)
	{
		return (Mathf.Abs(vector.x) > Mathf.Abs(vector.z));
	}
	
	/// <summary>
	/// Rotates a vector to current camera perspective while keeping y value
	/// </summary>
	/// <param name="vector">vector to rotate</param>
	/// <returns>Returns rotated vector</returns>
	public static Vector3 ApplyCameraRotation(Vector3 vector)
	{
		Camera cam = Camera.main;
		Vector3 camForward = cam.transform.forward;
		camForward.y = 0;
		camForward.Normalize();
		Vector3 camRight = cam.transform.right;
		camRight.y = 0;
		camRight.Normalize();
		Vector3 rotatedVector = vector.x * camRight + vector.y * Vector3.up + vector.z * camForward;
		return rotatedVector;
	}
	
	/// <summary>
	/// Makes a transform look at a position while aligning to closest axis (x/z)
	/// </summary>
	/// <param name="transform">transform to rotate</param>
	/// <param name="vector">vector to look at</param>
	public static void AxisAlignTo(Transform transform, Vector3 vector)
	{
		vector.y = 0;
		if (AlignsToXAxis(vector))
		{
			vector.z = 0;
		}
		else
		{
			vector.x = 0;
		}
		transform.LookAt(transform.position + vector);
	}
	
	/// <summary>
	/// Makes a transform look at a RigidBody while aligning to closest axis (x/z)
	/// </summary>
	/// <param name="transform">transform to rotate</param>
	/// <param name="rb">RigidBody to look at</param>
	public static void AxisAlignTo(Transform transform, Rigidbody rb)
	{
		AxisAlignTo(transform, rb.position - transform.position);
	}
}

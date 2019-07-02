using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMath
{
	public static bool AlignsToXAxis(Vector3 vector)
	{
		if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z)) return true;
		return false;
	}
	
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
	
	public static void AxisAlignTo(Transform transform, Rigidbody rb)
	{
		AxisAlignTo(transform, rb.position - transform.position);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAble : MonoBehaviour
{
    private Rigidbody rb;
    private float yForce;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Bounce(float triggerVelocityThreshhold, float maxBounceStr, float bounceIncrease)
    {
        if (rb.velocity.y < -triggerVelocityThreshhold)
        {
            yForce = Mathf.Min((rb.velocity.y * -bounceIncrease), maxBounceStr);
            rb.AddForce(new Vector3(0, yForce, 0));
        }
    }
}

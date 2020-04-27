using System.Collections;
using UnityEngine;

public class AirstreamFan : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float minRotationSpeed = 0.1f;

    [SerializeField]
    private float maxRotationSpeed = 10;

    [SerializeField]
    private float transitionDuration = 3;

    public void Initialize(bool active)
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.maxAngularVelocity = maxRotationSpeed;
        Toggle(active);
    }

    public void Toggle(bool active)
    {
        if (active)
        {
            StartCoroutine(TurnOn());
        }
        else
        {
            StartCoroutine(TurnOff());
        }
    }

    /// <summary>
    /// Accelerates angular velocity to maxRotationSpeed for transitionDuration s
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurnOn()
    {
        float timer = 0;
        while (timer < transitionDuration)
        {
            rb.angularVelocity = transform.forward * (timer / transitionDuration) * maxRotationSpeed;
            timer += Time.deltaTime;
            yield return null;
        }
        rb.angularVelocity = transform.forward * maxRotationSpeed;
    }

    /// <summary>
    /// Deccelerates angular velocity to maxRotationSpeed for transitionDuration s
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurnOff()
    {
        float timer = transitionDuration;
        while (timer > 0)
        {
            rb.angularVelocity = transform.forward * (timer / transitionDuration) * maxRotationSpeed;
            if (rb.angularVelocity.z < minRotationSpeed)
            {
                rb.angularVelocity = transform.forward * minRotationSpeed;
                yield break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
    }
}

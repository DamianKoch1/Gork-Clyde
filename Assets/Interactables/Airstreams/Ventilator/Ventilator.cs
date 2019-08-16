using System.Collections;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] 
    private float maxRotationSpeed = 10, minRotationSpeed = 0.1f, transitionDuration = 3;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.maxAngularVelocity = maxRotationSpeed;
        StartCoroutine(TurnOn());
    }
    
    /// <summary>
    /// Accelerates angular velocity to maxRotationSpeed for transitionDuration s
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurnOn()
    {
        StopCoroutine(TurnOff());
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
        StopCoroutine(TurnOn());
        float timer = transitionDuration;
        while (timer > 0)
        {
            rb.angularVelocity = transform.forward * (timer / transitionDuration) *  maxRotationSpeed;
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

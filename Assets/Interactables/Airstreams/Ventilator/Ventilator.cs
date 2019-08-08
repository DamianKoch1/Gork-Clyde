using System.Collections;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] 
    private float maxRotationSpeed = 10, minRotationSpeed = 0.1f, transitionDuration = 3;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.maxAngularVelocity = maxRotationSpeed;
        StartCoroutine(TurnOn());
    }

    public IEnumerator TurnOn()
    {
        print("start");
        StopCoroutine(TurnOff());
        float timer = 0;
        while (timer < transitionDuration)
        {
            rb.angularVelocity = new Vector3(0, 0, (timer / transitionDuration) * maxRotationSpeed);
            print("on, " + rb.angularVelocity.z);
            timer += Time.deltaTime;
            yield return null;
        }
        rb.angularVelocity = new Vector3(0, 0, maxRotationSpeed);
    }

    public IEnumerator TurnOff()
    {
        print("stop");
        StopCoroutine(TurnOn());
        float timer = transitionDuration;
        while (timer > 0)
        {
            print("off, " + rb.angularVelocity.z);
            rb.angularVelocity = new Vector3(0, 0, (timer / transitionDuration) *  maxRotationSpeed);
            if (rb.angularVelocity.z < minRotationSpeed)
            {
                rb.angularVelocity = new Vector3(0, 0, minRotationSpeed);
                yield break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
    }
}

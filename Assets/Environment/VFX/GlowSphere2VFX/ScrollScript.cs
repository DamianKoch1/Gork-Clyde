using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotateSpeed;
    void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.Self);
    }
}

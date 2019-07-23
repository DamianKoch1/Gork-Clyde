using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : MonoBehaviour
{
 private void Update()
 {
  transform.forward = Camera.main.transform.forward;
 }
}

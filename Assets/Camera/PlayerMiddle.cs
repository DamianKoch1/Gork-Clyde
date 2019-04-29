using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiddle : MonoBehaviour
{
   [SerializeField] 
   private GameObject gork, clide;

   private Vector3 playerMiddle;

   private void Update()
   {
      playerMiddle = 0.5f * (gork.transform.position + clide.transform.position);
      transform.position = playerMiddle;
   }
}

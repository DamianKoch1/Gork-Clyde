using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiddle : MonoBehaviour
{
   [SerializeField] 
   private GameObject gork, clyde;

   private Vector3 playerMiddle;

   private void Update()
   {
      MoveToMiddle();
   }

   private void MoveToMiddle()
   {
      if (!clyde.GetComponent<Player>().canMove)
      {
         transform.position = gork.transform.position;
      }
      else if (!gork.GetComponent<Player>().canMove)
      {
         transform.position = clyde.transform.position;
      }
      else
      {
         playerMiddle = 0.5f * (gork.transform.position + clyde.transform.position);
         transform.position = playerMiddle;
      }
   }
}

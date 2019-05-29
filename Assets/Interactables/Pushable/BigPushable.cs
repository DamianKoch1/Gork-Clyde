using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPushable : Pushable
{
     public new bool isPushed;
     
     private void OnTriggerStay(Collider other)
     {
          var gork = other.GetComponent<Gork>();
          if (gork)
          {
               if (!gork.pushedObj)
               {
                    gork.pushedObj = gameObject;
               }
          }
     }
     
     private void OnTriggerExit(Collider other)
     {
          var gork = other.GetComponent<Gork>();
          if (gork)
          {
               if (gork.pushedObj)
               {
                    gork.pushedObj = null;
               }
          }
     }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPushable : Pushable
{
     
     private void OnTriggerStay(Collider other)
     {
          var gork = other.GetComponent<Gork>();
          if (!gork) return;
          gork.pushedObj = gameObject;
     }
     
     private void OnTriggerExit(Collider other)
     {
          var gork = other.GetComponent<Gork>();
          if (!gork) return;
          if (!gork.pushedObj) return;
          gork.pushedObj = null;
     }

}

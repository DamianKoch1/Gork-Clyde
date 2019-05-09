using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

   private int playerCount = 0;
   [SerializeField] 
   private string nextLevelName;
   
   private void OnTriggerEnter(Collider other)
   {
      if (!other.isTrigger && other.GetComponent<Player>())
      {
         other.GetComponent<Player>().canMove = false;
         if (other.GetComponent<Player>().anim)
         {
            other.GetComponent<Player>().anim.enabled = false;
         }
         if (playerCount == 1)
         {
            LoadNextLevel();
         }
         else
         {
            playerCount++;
         }
      }
   }

   void LoadNextLevel()
   {
      SceneManager.LoadScene(nextLevelName);
   }
   
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

   private int enteredPlayerCount = 0;
   [SerializeField] 
   private string nextLevelName;
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.isTrigger) return;
      var player = other.GetComponent<Player>();
      if (!player) return;
      
      player.canMove = false;
      player.anim.enabled = false;
      StopCoroutine(player.CheckSpawnPoint());
      enteredPlayerCount++;
      if (enteredPlayerCount == 2)
      {
         LoadNextLevel();
      }
   }

   void LoadNextLevel()
   {
      CameraBehaviour.NEXT_SCENE_NAME = nextLevelName;
      CameraBehaviour.FADE_TO_BLACK();
   }
   
}


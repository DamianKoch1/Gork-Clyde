using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //player.anim.enabled = false;
        StopCoroutine(player.GetComponent<Respawning>().CheckSpawnPoint());
        enteredPlayerCount++;
        if (enteredPlayerCount == 2)
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        Fade.NEXT_SCENE_NAME = nextLevelName;
        Fade.FADE_TO_BLACK();
    }

}


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

        player.state.canMove = false;
        StopCoroutine(player.GetComponent<Respawning>().UpdateSpawnPoint());
        enteredPlayerCount++;
        if (enteredPlayerCount == 2)
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        LoadingScreen.NextLevelName = nextLevelName;
        Fade.FadeToBlack("Loading Screen");
    }

    
}


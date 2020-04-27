using UnityEngine;

public class Goal : MonoBehaviour
{

    /// <summary>
    /// Count of players currently in goal
    /// </summary>
    private int enteredPlayerCount = 0;
    
    /// <summary>
    /// Level that will be loaded when both players are in goal
    /// </summary>
    [SerializeField]
    private string nextLevelName;

    /// <summary>
    /// Disables entered players' movement and spawnpoint updating, loads nect level if 2 players are in
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        var player = other.GetComponent<Player>();
        if (!player) return;

        player.canMove = false;
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


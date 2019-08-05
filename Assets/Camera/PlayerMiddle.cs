using UnityEngine;

/// <summary>
/// Keeps an opject in the middle of both players, currently used as a camera target
/// </summary>
public class PlayerMiddle : MonoBehaviour
{
    [SerializeField]
    private GameObject gork, clyde;

    private Vector3 playerMiddle;

    private void Update()
    {
        MoveToMiddle();
    }

    
    /// <summary>
    /// Moves between players
    /// </summary>
    private void MoveToMiddle()
    {
        if (!clyde.GetComponent<PlayerState>().canMove)
        {
            transform.position = gork.transform.position;
        }
        else if (!gork.GetComponent<PlayerState>().canMove)
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

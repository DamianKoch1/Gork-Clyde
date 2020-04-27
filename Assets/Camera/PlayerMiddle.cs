using UnityEngine;

/// <summary>
/// Keeps an opject in the middle of both players, useful as camera target
/// </summary>
public class PlayerMiddle : MonoBehaviour
{
    [SerializeField]
    private Player gork;

    [SerializeField]
    private Player clyde;

    private Vector3 playerMiddle;

    private void Update()
    {
        MoveToMiddle();
    }

    
    private void MoveToMiddle()
    {
        if (!clyde.canMove)
        {
            transform.position = gork.transform.position;
        }
        else if (!gork.canMove)
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

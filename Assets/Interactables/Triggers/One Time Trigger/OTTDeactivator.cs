using UnityEngine;

/// <summary>
/// Deactivates targets once if both players enter
/// </summary>
public class OTTDeactivator : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] targets;
    
    private int playersInTrigger = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        playersInTrigger++;
        if (playersInTrigger == 2)
        {
            foreach (var target in targets)
            {
                target.GetComponent<IActivatable>()?.OnButtonDeactivated();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Player>()) return;
        playersInTrigger--;
    }
}

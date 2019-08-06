using UnityEngine;

public class BlockableAirstream : MonoBehaviour
{
    /// <summary>
    /// The airstream that can be blocked
    /// </summary>
    [SerializeField]
    private Airstream blockableAirstream;

    /// <summary>
    /// Toggles blockableAirstream if "platform" tagged object enters trigger
    /// </summary>
    /// <param name="other">Collider to check for platform tag</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("platform"))
        {
            blockableAirstream.ToggleAirstream();
        }
    }

    /// <summary>
    /// Toggles blockableAirstream if "platform" tagged object exits trigger
    /// </summary>
    /// <param name="other">Collider to check for platform tag</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("platform"))
        {
            blockableAirstream.ToggleAirstream();
        }
    }
}

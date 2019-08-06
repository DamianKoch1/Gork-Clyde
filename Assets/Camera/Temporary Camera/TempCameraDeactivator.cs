using UnityEngine;

/// <summary>
/// Deactivates camera if a player exits trigger
/// </summary>
public class TempCameraDeactivator : MonoBehaviour
{
    private TempCamera cam;
    
    private void Start()
    {
        cam = GetComponentInParent<TempCamera>();
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (!cam.CanActivateCam(other)) return;

        cam.DeactivateCamera();
    }
}

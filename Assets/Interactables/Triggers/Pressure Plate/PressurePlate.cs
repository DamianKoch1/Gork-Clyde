using UnityEngine;

/// <summary>
/// Activates objects while player/pushable is on plate
/// </summary>
public class PressurePlate : TriggerObject
{

    private int objectsOnPlateCount = 0;

    [SerializeField]
    private AudioClip activateSFX, deactivateSFX;


    private void OnTriggerExit(Collider other)
    {
        if (!MatchesTriggerCondition(other)) return;
        OnPlateExited();
    }

    /// <summary>
    /// Activates targets if plate was empty
    /// </summary>
    protected override void OnTriggered()
    {
        if (objectsOnPlateCount == 0)
        {
            GetComponent<Animator>().SetBool("OnPlate", true);
            SetCableMaterial(activeMat);
            GetComponent<AudioSource>().PlayOneShot(activateSFX);
            foreach (GameObject target in targets)
            {
                target.GetComponent<IActivatable>()?.OnPlateActivated();
            }
        }
        objectsOnPlateCount++;

    }

    /// <summary>
    /// Deactivates targets if last object exited
    /// </summary>
    private void OnPlateExited()
    {
        if (objectsOnPlateCount == 1)
        {
            GetComponent<Animator>().SetBool("OnPlate", false);
            SetCableMaterial(inactiveMat);
            GetComponent<AudioSource>().PlayOneShot(deactivateSFX);
            foreach (GameObject target in targets)
            {
                target.GetComponent<IActivatable>()?.OnPlateExited();
            }
        }
        objectsOnPlateCount--;
    }

}

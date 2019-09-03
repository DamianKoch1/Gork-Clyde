using UnityEngine;

/// <summary>
/// Used to show button hints in tutorial, shows/hides if player enters/exits trigger
/// </summary>
public class ButtonHint : MonoBehaviour
{

    private Animator anim;

    private int playersInTriggerCount = 0;

    private bool active;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        RotateToCamera();
    }

    /// <summary>
    /// Sets forward vector to forward of current camera
    /// </summary>
    private void RotateToCamera()
    {
        transform.forward = Camera.main.transform.forward;
    }

    /// <summary>
    /// Activate on player enter
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        playersInTriggerCount++;

        active = true;
        anim.SetBool("active", true);
    }

    
    /// <summary>
    /// Deactivate on both player exit
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        playersInTriggerCount--;

        if (playersInTriggerCount == 0)
        {
            active = false;
            anim.SetBool("active", false);
        }
    }
}

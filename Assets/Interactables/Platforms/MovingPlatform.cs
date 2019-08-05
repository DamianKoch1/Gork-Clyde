using UnityEngine;

public class MovingPlatform : MonoBehaviour, IActivatable
{
    [SerializeField]
    private GameObject start, target, platform;
    private Vector3 pos1, pos2;
    private bool stop, blocked;
    public enum Mode { Autostart, Triggerable, MoveOnce};
    public Mode mode = Mode.Autostart;
    [SerializeField]
    private float speed;
    private Rigidbody rb;
    private float moveAmount = 0;


    private void Start()
    {
        InitializeVariables();
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }
 
    private void InitializeVariables()
    {
        rb = platform.GetComponent<Rigidbody>();
        pos1 = start.transform.position;
        pos2 = target.transform.position;
        if (mode != Mode.Autostart)
        {
            stop = true;
        }
    }

    /// <summary>
    /// Moves between pos1 / pos2 on a sinus curve if activated & not blocked
    /// </summary>
    private void MovePlatform()
    {
        if (stop) return;
        if (blocked) return;
        if (mode == Mode.MoveOnce)
        {
            if (Vector3.Distance(rb.position, pos2) < 0.1f) return;
        }

        if (moveAmount >= Mathf.PI * 200)
        {
            moveAmount = 0;
        }
        rb.MovePosition(pos1 + 0.5f * (1 + Mathf.Sin(moveAmount - Mathf.PI / 2)) * (pos2 - pos1));
        moveAmount += Time.deltaTime * speed;
    }


    public void Blocked()
    {
        blocked = true;
    }

    public void Unblocked()
    {
        blocked = false;
    }

    public void OnButtonActivated()
    {
        stop = false;
    }

    public void OnButtonDeactivated()
    {
        stop = true;
    }

    public void OnPlateActivated()
    {
        stop = false;
    }
    public void OnPlateExited()
    {
        stop = true;
    }
}

using UnityEngine;


public class DoorLockController : MonoBehaviour
{
    [Header("Door Components")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    public Rigidbody rb;
    public HingeJoint hinge;

    private JointLimits lockedLimits;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!hinge) hinge = GetComponent<HingeJoint>();
        if (!grabInteractable) grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Save locked hinge state (door cannot rotate)
        lockedLimits = hinge.limits;
        lockedLimits.min = 0f;
        lockedLimits.max = 0f;
    }

    // =======================
    // LOCK / UNLOCK
    // =======================

    public void LockDoor()
    {
        // Disable grabbing
        if (grabInteractable)
            grabInteractable.enabled = false;

        // Freeze door completely
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Lock hinge rotation
        if (hinge)
        {
            hinge.useLimits = true;
            hinge.limits = lockedLimits;
        }
    }

    public void UnlockDoor()
    {
        // Enable grabbing
        if (grabInteractable)
            grabInteractable.enabled = true;

        // Unfreeze door (allow hinge movement)
        if (rb)
        {
            rb.constraints = RigidbodyConstraints.None;
        }

        // Restore hinge rotation
        if (hinge)
        {
            hinge.useLimits = false;
        }
    }
}

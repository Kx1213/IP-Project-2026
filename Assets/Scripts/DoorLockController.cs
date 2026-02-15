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

        // Door cannot rotate
        lockedLimits = hinge.limits;
        lockedLimits.min = 0f;
        lockedLimits.max = 0f;
    }

    public void LockDoor()
    {
        // Disable grabbing
        if (grabInteractable)
            grabInteractable.enabled = false;

        // Freeze door in rigidbody
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

        // Unfreeze door 
        if (rb)
        {
            rb.constraints = RigidbodyConstraints.None;
        }

        // Unlock hinge rotation
        if (hinge)
        {
            hinge.useLimits = false;
        }
    }
}

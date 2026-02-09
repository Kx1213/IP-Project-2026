using UnityEngine;
#if UNITY_XR_INTERACTION_TOOLKIT
using UnityEngine.XR.Interaction.Toolkit;
#endif

public class DoorLockController : MonoBehaviour
{
    private Rigidbody rb;

#if UNITY_XR_INTERACTION_TOOLKIT
    private XRGrabInteractable grab;
#endif

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

#if UNITY_XR_INTERACTION_TOOLKIT
        grab = GetComponent<XRGrabInteractable>();
#endif

        LockDoor();
    }

    // Door blocks player but cannot move
    public void LockDoor()
    {
        if (rb)
        {
            rb.isKinematic = false; // KEEP physics enabled
            rb.constraints = RigidbodyConstraints.FreezeAll; // FREEZE movement
        }

#if UNITY_XR_INTERACTION_TOOLKIT
        if (grab)
            grab.enabled = false; // cannot grab
#endif
    }

    // Door becomes interactable again
    public void UnlockDoor()
    {
        if (rb)
        {
            rb.constraints = RigidbodyConstraints.None; // allow movement
        }

#if UNITY_XR_INTERACTION_TOOLKIT
        if (grab)
            grab.enabled = true;
#endif
    }
}

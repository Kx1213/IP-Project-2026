using UnityEngine;

public class PeelAndSwap : MonoBehaviour
{
    public BoilingStepManager manager;

    [Header("Peeling")]
    public int strokesNeeded = 8;
    private int strokes;

    [Header("Swap")]
    public GameObject peeledPotatoPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!manager.IsStep(BoilingStepManager.Step.PeelPotato))
            return;

        if (!other.CompareTag("Peeler"))
            return;

        strokes++;

        if (strokes >= strokesNeeded)
        {
            // Advance step first
            manager.AdvanceStep(BoilingStepManager.Step.PeelPotato);

            // Spawn peeled version
            GameObject newPotato = Instantiate(
                peeledPotatoPrefab,
                transform.position,
                transform.rotation
            );

            // Optional: keep same velocity if grabbed
            Rigidbody rbOld = GetComponent<Rigidbody>();
            Rigidbody rbNew = newPotato.GetComponent<Rigidbody>();

            if (rbOld && rbNew)
                rbNew.linearVelocity = rbOld.linearVelocity;

            Destroy(gameObject);
        }
    }
}

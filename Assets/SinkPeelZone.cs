using UnityEngine;

public class SinkPeelZone : MonoBehaviour
{
    [Header("References")]
    public BoilingStepManager manager;

    [Header("Required Tags")]
    public string potatoTag = "Potato";
    public string peelerTag = "Peeler";

    [Header("Peeling Progress")]
    public int strokesNeeded = 8;
    public float strokeCooldown = 0.12f; // prevents counting too fast

    [Header("Prefab Swap")]
    public GameObject peeledPotatoPrefab;

    private int strokes;
    private float lastStrokeTime;

    // We only peel the potato that is currently inside the sink zone
    private Transform potatoInSink;

    private void OnTriggerEnter(Collider other)
    {
        // Track potato entering sink
        if (other.CompareTag(potatoTag))
        {
            potatoInSink = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Potato left sink, stop peeling progress
        if (other.CompareTag(potatoTag) && potatoInSink == other.transform)
        {
            potatoInSink = null;
            strokes = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (manager == null) return;

        // Only during peel step
        if (!manager.IsStep(BoilingStepManager.Step.PeelPotato))
            return;

        // Must have a potato inside sink
        if (potatoInSink == null)
            return;

        // Only peeler strokes count
        if (!other.CompareTag(peelerTag))
            return;

        // Cooldown (avoid instant completion)
        if (Time.time - lastStrokeTime < strokeCooldown)
            return;

        lastStrokeTime = Time.time;
        strokes++;

        if (strokes >= strokesNeeded)
        {
            CompletePeelAndSwap();
        }
    }

    private void CompletePeelAndSwap()
    {
        if (peeledPotatoPrefab == null)
        {
            Debug.LogError("[SinkPeelZone] peeledPotatoPrefab not assigned!");
            return;
        }

        // Advance step first
        manager.AdvanceStep(BoilingStepManager.Step.PeelPotato);

        Vector3 pos = potatoInSink.position;
        Quaternion rot = potatoInSink.rotation;

        // Spawn peeled potato at same place
        GameObject peeled = Instantiate(peeledPotatoPrefab, pos, rot);

        // Optional: keep velocity so it feels smooth
        Rigidbody oldRb = potatoInSink.GetComponent<Rigidbody>();
        Rigidbody newRb = peeled.GetComponent<Rigidbody>();
        if (oldRb != null && newRb != null)
        {
            newRb.linearVelocity = oldRb.linearVelocity;
            newRb.angularVelocity = oldRb.angularVelocity;
        }

        // Destroy raw potato
        Destroy(potatoInSink.gameObject);

        // Reset zone state
        potatoInSink = null;
        strokes = 0;
    }
}

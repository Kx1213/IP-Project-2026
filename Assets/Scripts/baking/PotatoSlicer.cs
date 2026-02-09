using UnityEngine;

public class PotatoSlicer : MonoBehaviour
{
    [Header("References")]
    public CookingStepManager manager;
    public GameObject cutPotatoPrefab;
    public Transform spawnPoint;

    [Header("Slice Settings")]
    public int slicesNeeded = 10;

    private int slicesDone;
    private bool completed;

    private void OnTriggerEnter(Collider other)
    {
        if (completed) return;

        // Step gate
        if (!manager || !manager.IsStep(CookingStepManager.Step.Slice5mmDontCutThrough))
            return;

        // Only knife can cut
        if (!other.CompareTag("Knife"))
            return;

        slicesDone++;
        Debug.Log($"Slice {slicesDone}/{slicesNeeded}");

        if (slicesDone >= slicesNeeded)
        {
            completed = true;
            FinishCutting();
        }
    }

    private void FinishCutting()
    {
        // Advance recipe step
        manager.AdvanceStep(CookingStepManager.Step.Slice5mmDontCutThrough);

        // Spawn sliced potato
        Vector3 pos = spawnPoint ? spawnPoint.position : transform.parent.position;
        Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.parent.rotation;

        Instantiate(cutPotatoPrefab, pos, rot);

        // Destroy RAW potato (parent)
        Destroy(transform.parent.gameObject);
    }
}

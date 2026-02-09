using UnityEngine;

public class PotatoSlicer : MonoBehaviour
{
    public CookingStepManager manager;

    [Header("Slice Requirement")]
    public int slicesNeeded = 10;
    private int slicesDone;
    private bool sliced;

    [Header("Prefab Swap")]
    public GameObject cutPotatoPrefab;
    public Transform spawnPoint; // optional

    private void OnTriggerEnter(Collider other)
    {
        if (sliced) return;

        if (!manager.IsStep(CookingStepManager.Step.Slice5mmDontCutThrough))
            return;

        if (!other.CompareTag("Knife"))
            return;

        slicesDone++;
        Debug.Log($"Slicing {slicesDone}/{slicesNeeded}");

        if (slicesDone >= slicesNeeded)
        {
            sliced = true;

            manager.AdvanceStep(CookingStepManager.Step.Slice5mmDontCutThrough);

            Vector3 pos = spawnPoint ? spawnPoint.position : transform.position;
            Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;

            Instantiate(cutPotatoPrefab, pos, rot);
            Destroy(gameObject);
        }
    }
}

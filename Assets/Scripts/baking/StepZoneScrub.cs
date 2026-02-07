using UnityEngine;

public class StepZoneScrub : MonoBehaviour
{
    public RecipeManager manager;
    public float scrubTime = 2f;
    float t;

    private void Awake()
    {
        if (manager == null) manager = FindFirstObjectByType<RecipeManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!manager.IsStep(1)) return;
        if (!other.CompareTag("PotatoRaw")) return;

        t += Time.deltaTime;
        if (t >= scrubTime)
        {
            t = 0;
            manager.CompleteStep(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PotatoRaw")) t = 0;
    }
}

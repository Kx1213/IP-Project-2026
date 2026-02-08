using UnityEngine;

public class StepZoneCut : MonoBehaviour
{
    public RecipeManager manager;
    public float cutTime = 1.5f;
    float t;

    private void Awake()
    {
        if (manager == null) manager = FindFirstObjectByType<RecipeManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!manager.IsStep(2)) return;

        // We need potato inside zone
        if (other.CompareTag("PotatoRaw"))
        {
            // Cutting happens when KnifeTip is also touching potato,
            // so we just wait time while potato stays in zone,
            // and player must be "using" knife tip on potato.
            t += Time.deltaTime;
            if (t >= cutTime)
            {
                t = 0f;
                manager.CompleteStep(2);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PotatoRaw")) t = 0;
    }
}

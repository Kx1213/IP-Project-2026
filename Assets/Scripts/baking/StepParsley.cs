using UnityEngine;

public class StepParsley : MonoBehaviour
{
    public RecipeManager manager;

    private void Awake()
    {
        if (manager == null) manager = FindFirstObjectByType<RecipeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!manager.IsStep(7)) return; // if you used step 7
        if (other.CompareTag("Parsley"))
        {
            Debug.Log("✅ Parsley added. DONE!");
            manager.CompleteStep(7);
        }
    }
}

using UnityEngine;

public class StepButton : MonoBehaviour
{
    public RecipeManager manager;
    public int requiredStep; // must match current step

    private void Awake()
    {
        if (manager == null) manager = FindFirstObjectByType<RecipeManager>();
    }

    public void Press()
    {
        if (!manager.IsStep(requiredStep))
        {
            Debug.Log("❌ Not the right step yet.");
            return;
        }

        manager.CompleteStep(requiredStep);
    }
}

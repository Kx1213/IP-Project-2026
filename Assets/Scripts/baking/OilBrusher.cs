using UnityEngine;

public class OilBrusher : MonoBehaviour
{
    public CookingStepManager manager;

    public int strokesNeeded = 8;
    private int strokesDone;

    private void OnTriggerEnter(Collider other)
    {
        if (!manager.IsStep(CookingStepManager.Step.BrushWithOilOrButter))
            return;

        if (!other.CompareTag("Brush"))
            return;

        strokesDone++;

        if (strokesDone >= strokesNeeded)
            manager.AdvanceStep(CookingStepManager.Step.BrushWithOilOrButter);
    }
}

using UnityEngine;

public class OvenPreheat200 : MonoBehaviour
{
    public CookingStepManager manager;

    public void SetTo200()
    {
        if (!manager.IsStep(CookingStepManager.Step.PreheatOven200))
            return;

        manager.AdvanceStep(CookingStepManager.Step.PreheatOven200);
    }
}

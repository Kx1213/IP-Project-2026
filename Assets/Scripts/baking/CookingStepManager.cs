using UnityEngine;
using UnityEngine.Events;

public class CookingStepManager : MonoBehaviour
{
    public enum Step
    {
        PreheatOven200,          // Step 1
        ScrubSkinNoPeel,         // Step 2
        Slice5mmDontCutThrough,  // Step 3
        PlaceInBakingPan,        // Step 4
        BrushWithOilOrButter,    // Step 5a
        SprinkleSalt,            // Step 5b
        Bake40Min,               // Step 6
        ServeOnPlate,            // Step 7a
        GarnishParsley,          // Step 7b
        Done
    }

    [Header("Current Step")]
    public Step currentStep = Step.PreheatOven200;

    [Header("Optional Event (for UI later)")]
    public UnityEvent<Step> OnStepChanged;

    public bool IsStep(Step step)
    {
        return currentStep == step;
    }

    public void AdvanceStep(Step expectedCurrent)
    {
        if (currentStep != expectedCurrent) return;

        currentStep = (Step)((int)currentStep + 1);

        Debug.Log("STEP -> " + currentStep);
        OnStepChanged?.Invoke(currentStep);
    }
}

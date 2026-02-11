using UnityEngine;
using UnityEngine.Events;

public class CookingStepManager : MonoBehaviour
{
    public enum Step
    {
        PreheatOven200,
        ScrubSkinNoPeel,
        Slice5mmDontCutThrough,
        PlaceInBakingPan,
        BrushWithOilOrButter,
        SprinkleSalt,
        Bake40Min,
        ServeOnPlate,
        Done
    }

    [System.Serializable]
    public class StepEvent : UnityEvent<Step> { }

    [Header("Current Step")]
    public Step currentStep = Step.PreheatOven200;

    [Header("UI event (never null)")]
    public StepEvent OnStepChanged = new StepEvent();

    [Header("Baking UI helper")]
    public bool bakingInProgress;

    public bool IsStep(Step step) => currentStep == step;

    public void AdvanceStep(Step expectedCurrent)
    {
        if (currentStep != expectedCurrent) return;
        if (currentStep == Step.Done) return;

        currentStep = (Step)((int)currentStep + 1);

        Debug.Log("STEP -> " + currentStep);
        OnStepChanged.Invoke(currentStep);
    }

    public void SetBakingInProgress(bool value)
    {
        bakingInProgress = value;
        // Force UI refresh even if step didn't change
        OnStepChanged.Invoke(currentStep);
    }

    public void FinishGame(Step expectedCurrent)
    {
        if (currentStep != expectedCurrent) return;

        currentStep = Step.Done;
        bakingInProgress = false;

        Debug.Log("GAME FINISHED!");
        OnStepChanged.Invoke(currentStep);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class BoilingStepManager : MonoBehaviour
{
    public enum Step
    {
        PeelPotato,
        CutSmallPieces,
        AddSaltToWater,
        PressBoil,
        AddPotatoToPot,
        Boil20Min,
        TakePotatoOut,
        SeasonSaltOil,
        Done
    }

    [System.Serializable]
    public class StepEvent : UnityEvent<Step> { }

    public Step currentStep = Step.PeelPotato;
    public StepEvent OnStepChanged = new StepEvent();

    public bool boilingInProgress;

    public bool IsStep(Step step) => currentStep == step;

    public void AdvanceStep(Step expected)
    {
        if (currentStep != expected) return;
        if (currentStep == Step.Done) return;

        currentStep = (Step)((int)currentStep + 1);
        Debug.Log("STEP -> " + currentStep);
        OnStepChanged.Invoke(currentStep);
    }

    public void SetBoilingInProgress(bool value)
    {
        boilingInProgress = value;
        OnStepChanged.Invoke(currentStep);
    }

    public void Finish()
    {
        currentStep = Step.Done;
        boilingInProgress = false;
        OnStepChanged.Invoke(currentStep);
    }
}

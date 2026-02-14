using System.Collections;
using UnityEngine;

public class BoilButton : MonoBehaviour
{
    public BoilingStepManager manager;
    public float boilSeconds = 10f; // demo for 20 minutes

    private bool boiling;

    public void PressBoil()
    {
        if (boiling) return;

        if (manager.IsStep(BoilingStepManager.Step.PressBoil))
        {
            manager.AdvanceStep(BoilingStepManager.Step.PressBoil);
            return;
        }

        if (!manager.IsStep(BoilingStepManager.Step.Boil20Min))
            return;

        boiling = true;
        manager.SetBoilingInProgress(true);
        StartCoroutine(BoilRoutine());
    }

    private IEnumerator BoilRoutine()
    {
        yield return new WaitForSeconds(boilSeconds);

        manager.SetBoilingInProgress(false);
        manager.AdvanceStep(BoilingStepManager.Step.Boil20Min);

        boiling = false;
    }
}

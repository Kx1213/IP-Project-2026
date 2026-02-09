using UnityEngine;

public class ParsleyGarnishZone : MonoBehaviour
{
    public CookingStepManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (!manager.IsStep(CookingStepManager.Step.GarnishParsley))
            return;

        if (!other.CompareTag("Parsley"))
            return;

        manager.AdvanceStep(CookingStepManager.Step.GarnishParsley);
    }
}

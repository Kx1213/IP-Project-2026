using UnityEngine;

public class SaltToWaterZone : MonoBehaviour
{
    public BoilingStepManager manager;
    public float secondsNeeded = 2f;

    private float timer;

    private void OnTriggerStay(Collider other)
    {
        if (!manager.IsStep(BoilingStepManager.Step.AddSaltToWater))
            return;

        if (!other.CompareTag("SaltShaker"))
            return;

        timer += Time.deltaTime;

        if (timer >= secondsNeeded)
        {
            manager.AdvanceStep(BoilingStepManager.Step.AddSaltToWater);
            enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SaltShaker"))
            timer = 0f;
    }
}

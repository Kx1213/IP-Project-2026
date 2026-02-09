using UnityEngine;

public class PotatoScrubZone : MonoBehaviour
{
    public CookingStepManager manager;

    public string potatoTag = "Potato";
    public float scrubSeconds = 3f;

    private float timer;

    private void OnTriggerStay(Collider other)
    {
        if (!manager.IsStep(CookingStepManager.Step.ScrubSkinNoPeel))
            return;

        if (!other.CompareTag(potatoTag))
            return;

        timer += Time.deltaTime;

        if (timer >= scrubSeconds)
        {
            manager.AdvanceStep(CookingStepManager.Step.ScrubSkinNoPeel);
            enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(potatoTag))
            timer = 0f;
    }
}
